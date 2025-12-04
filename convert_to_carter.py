#!/usr/bin/env python3
"""
Convert endpoint files from static extension methods to ICarterModule pattern.
"""

import os
import re
from pathlib import Path

def find_endpoint_files(base_path):
    """Find all *Endpoints.cs files."""
    endpoints = []
    for root, dirs, files in os.walk(base_path):
        for file in files:
            if file.endswith('Endpoints.cs') and '/v1/' not in root:
                endpoints.append(os.path.join(root, file))
    return endpoints

def read_v1_endpoint_files(endpoints_dir):
    """Read all individual endpoint files in v1 subdirectory."""
    v1_dir = os.path.join(endpoints_dir, 'v1')
    if not os.path.exists(v1_dir):
        return []
    
    endpoint_mappings = []
    for file in sorted(os.listdir(v1_dir)):
        if file.endswith('Endpoint.cs'):
            filepath = os.path.join(v1_dir, file)
            with open(filepath, 'r') as f:
                content = f.read()
                endpoint_mappings.append({
                    'file': file,
                    'content': content
                })
    return endpoint_mappings

def extract_endpoint_info(content):
    """Extract HTTP method, route, handler info from endpoint file."""
    info = {}
    
    # Extract HTTP method
    method_match = re.search(r'\.Map(Post|Get|Put|Delete|Patch)\(', content)
    if method_match:
        info['method'] = method_match.group(1)
    
    # Extract route
    route_match = re.search(r'\.Map(?:Post|Get|Put|Delete|Patch)\("([^"]*)"', content)
    if route_match:
        info['route'] = route_match.group(1)
    
    # Extract handler code (between MapX and .WithName)
    handler_match = re.search(r'\.Map(?:Post|Get|Put|Delete|Patch)\([^)]+\)\s*,\s*async\s*\(([^)]+)\)\s*=>\s*\{([^}]+(?:\{[^}]*\}[^}]*)*)\}', content, re.DOTALL)
    if handler_match:
        info['params'] = handler_match.group(1)
        info['handler_body'] = handler_match.group(2).strip()
    
    # Extract WithName
    name_match = re.search(r'\.WithName\(([^)]+)\)', content)
    if name_match:
        info['name'] = name_match.group(1)
    
    # Extract WithSummary
    summary_match = re.search(r'\.WithSummary\("([^"]+)"\)', content)
    if summary_match:
        info['summary'] = summary_match.group(1)
    
    # Extract Produces
    produces_match = re.search(r'\.Produces<([^>]+)>\(([^)]*)\)', content)
    if produces_match:
        info['produces_type'] = produces_match.group(1)
        info['produces_code'] = produces_match.group(2) if produces_match.group(2) else ''
    
    # Extract using statements
    using_matches = re.findall(r'using ([^;]+);', content)
    info['usings'] = using_matches
    
    return info

def generate_carter_module(main_file_path, v1_endpoints):
    """Generate ICarterModule content from v1 endpoint files."""
    with open(main_file_path, 'r') as f:
        content = f.read()
    
    # Extract namespace and class info
    namespace_match = re.search(r'namespace\s+([^;]+);', content)
    namespace = namespace_match.group(1) if namespace_match else ''
    
    # Extract XML documentation
    xml_doc_match = re.search(r'(///[^\n]*\n)+', content)
    xml_doc = xml_doc_match.group(0) if xml_doc_match else ''
    
    # Extract class name and description
    class_match = re.search(r'public\s+static\s+class\s+(\w+)', content)
    class_name = class_match.group(1) if class_match else 'Endpoints'
    
    # Extract route group info
    group_match = re.search(r'\.MapGroup\("([^"]+)"\)', content)
    route_prefix = group_match.group(1) if group_match else '/'
    
    # Extract tags
    tag_match = re.search(r'\.WithTags\("([^"]+)"\)', content)
    tag = tag_match.group(1) if tag_match else class_name.replace('Endpoints', '')
    
    # Collect all using statements from v1 endpoints
    all_usings = set()
    all_usings.add('using Carter;')
    all_usings.add('using MediatR;')
    all_usings.add('using Microsoft.AspNetCore.Builder;')
    all_usings.add('using Microsoft.AspNetCore.Http;')
    all_usings.add('using Microsoft.AspNetCore.Routing;')
    
    for endpoint in v1_endpoints:
        info = extract_endpoint_info(endpoint['content'])
        for using in info.get('usings', []):
            if 'Application' in using or 'Authorization' in using:
                all_usings.add(f'using {using};')
    
    # Generate new content
    new_content = '\n'.join(sorted(all_usings)) + '\n\n'
    new_content += f'namespace {namespace};\n\n'
    new_content += xml_doc
    new_content += f'public class {class_name} : ICarterModule\n{{\n'
    new_content += f'    public void AddRoutes(IEndpointRouteBuilder app)\n    {{\n'
    new_content += f'        var group = app.MapGroup("{route_prefix}").WithTags("{tag}");\n\n'
    
    # Add each endpoint
    for endpoint in v1_endpoints:
        info = extract_endpoint_info(endpoint['content'])
        if not info.get('method'):
            continue
            
        method = info['method']
        route = info.get('route', '/')
        params = info.get('params', '')
        handler_body = info.get('handler_body', '')
        name = info.get('name', f'"{endpoint["file"].replace("Endpoint.cs", "")}"')
        summary = info.get('summary', '')
        produces_type = info.get('produces_type', '')
        produces_code = info.get('produces_code', '')
        
        # Clean up handler body
        handler_body = re.sub(r'^\s+', '                ', handler_body, flags=re.MULTILINE)
        
        new_content += f'        group.Map{method}("{route}", async ({params}) =>\n'
        new_content += '            {\n'
        new_content += f'{handler_body}\n'
        new_content += '            })\n'
        new_content += f'            .WithName({name})\n'
        if summary:
            new_content += f'            .WithSummary("{summary}")\n'
        if produces_type:
            if produces_code:
                new_content += f'            .Produces<{produces_type}>({produces_code});\n\n'
            else:
                new_content += f'            .Produces<{produces_type}>();\n\n'
        else:
            new_content += ';\n\n'
    
    new_content += '    }\n'
    new_content += '}\n'
    
    return new_content

# Main execution
if __name__ == '__main__':
    base_dir = Path(__file__).parent / 'src' / 'api' / 'modules'
    
    modules = [
        base_dir / 'Store' / 'Store.Infrastructure' / 'Endpoints',
        base_dir / 'HumanResources' / 'Hr.Infrastructure' / 'Endpoints',
        base_dir / 'Accounting' / 'Accounting.Infrastructure' / 'Endpoints'
    ]
    
    for module_path in modules:
        if not module_path.exists():
            continue
            
        endpoint_files = find_endpoint_files(str(module_path))
        print(f"\n{module_path.name} - Found {len(endpoint_files)} endpoint files")
        
        for endpoint_file in endpoint_files:
            print(f"  Processing: {Path(endpoint_file).relative_to(module_path)}")
            endpoints_dir = Path(endpoint_file).parent
            v1_endpoints = read_v1_endpoint_files(str(endpoints_dir))
            
            if v1_endpoints:
                print(f"    Found {len(v1_endpoints)} v1 endpoints")
                # Generate would go here - for now just report
            else:
                print(f"    No v1 endpoints found")

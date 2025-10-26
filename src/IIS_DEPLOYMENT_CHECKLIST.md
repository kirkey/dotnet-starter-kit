# IIS Deployment Checklist

Use this checklist to ensure a successful deployment of your FSH Starter Kit to IIS.

## Phase 1: Pre-Deployment Preparation

### Development Environment
- [ ] All code changes committed and pushed
- [ ] Application builds successfully without errors
- [ ] All tests passing
- [ ] Code reviewed and approved
- [ ] Version tagged in source control

### Configuration Files
- [ ] `appsettings.Production.json` created for API
- [ ] Production database connection string configured
- [ ] JWT secret key set (strong, unique, 32+ characters)
- [ ] CORS settings configured with actual domain
- [ ] Mail settings configured (if using email)
- [ ] Blazor `appsettings.Production.json` created
- [ ] API base URL set to production API domain
- [ ] Swagger disabled in production settings

### Certificates & DNS
- [ ] SSL certificates obtained
- [ ] SSL certificates installed on IIS server
- [ ] DNS records configured for API domain
- [ ] DNS records configured for Blazor domain
- [ ] DNS propagation verified

## Phase 2: Server Setup

### Windows Server Prerequisites
- [ ] Windows Server 2016 or later installed
- [ ] Latest Windows updates applied
- [ ] IIS role installed and running
- [ ] .NET 9.0 Hosting Bundle installed
- [ ] URL Rewrite Module installed
- [ ] IIS restarted after installing prerequisites
- [ ] Server has internet connectivity (for NuGet restore if needed)

### Verification Commands
```powershell
# Run these commands and verify output:
- [ ] dotnet --list-runtimes  # Shows .NET 9.x runtimes
- [ ] Get-WindowsFeature Web-Server  # Shows IIS installed
- [ ] Get-Module -ListAvailable WebAdministration  # Shows IIS module
```

### Database Server
- [ ] Database server accessible from IIS server
- [ ] Test connection from IIS server to database
- [ ] Database created (or will be created by migrations)
- [ ] Database user account created
- [ ] Database user has appropriate permissions
- [ ] Firewall rules allow IIS → Database connection

### Networking
- [ ] Port 80 (HTTP) opened in firewall
- [ ] Port 443 (HTTPS) opened in firewall
- [ ] Database port opened (if on separate server)
- [ ] Server accessible from internet (if public deployment)
- [ ] Internal network routing configured (if internal)

## Phase 3: Build & Package

### Build Process
- [ ] Run `./publish-for-iis.sh` or `.\publish-for-iis.ps1`
- [ ] API build completed successfully
- [ ] Blazor build completed successfully
- [ ] ZIP packages created
- [ ] Verify package sizes are reasonable
- [ ] Test extract of packages locally

### Package Contents Verification
API Package should contain:
- [ ] FSH.Starter.WebApi.Host.dll
- [ ] appsettings.json
- [ ] web.config
- [ ] All dependency DLLs
- [ ] wwwroot folder (if any static files)

Blazor Package should contain:
- [ ] index.html
- [ ] appsettings.json
- [ ] web.config
- [ ] _framework folder with WebAssembly files
- [ ] CSS and JS files

## Phase 4: File Transfer

### Transfer to Server
- [ ] Packages copied to IIS server
- [ ] Deployment scripts copied (if using automated deployment)
- [ ] Configuration files copied or prepared
- [ ] Backup of current deployment taken (if updating)

## Phase 5: IIS Configuration

### API Deployment
- [ ] Application pool created: `FSH.Starter.API.Pool`
- [ ] App pool set to "No Managed Code"
- [ ] App pool identity configured
- [ ] Website/Application created: `FSH.Starter.API`
- [ ] Physical path set: `C:\inetpub\wwwroot\FSH.Starter.API`
- [ ] Files extracted to physical path
- [ ] Binding configured (HTTPS on port 443)
- [ ] SSL certificate bound to site
- [ ] Host header configured: `api.yourdomain.com`
- [ ] Logs folder created: `[physical-path]\logs`
- [ ] Permissions set: IIS_IUSRS has Full Control
- [ ] web.config verified in root folder
- [ ] Application pool started

### Blazor Deployment
- [ ] Application pool created: `FSH.Starter.Blazor.Pool`
- [ ] App pool set to "No Managed Code"
- [ ] Website created: `FSH.Starter.Blazor`
- [ ] Physical path set: `C:\inetpub\wwwroot\FSH.Starter.Blazor`
- [ ] Files extracted to physical path
- [ ] Binding configured (HTTPS on port 443)
- [ ] SSL certificate bound to site
- [ ] Host header configured: `app.yourdomain.com`
- [ ] web.config verified (with URL rewrite rules)
- [ ] Permissions set: IIS_IUSRS has Read access
- [ ] Application pool started

## Phase 6: Configuration Updates

### API Configuration
- [ ] Update `appsettings.json` with production values
- [ ] Database connection string set correctly
- [ ] JWT secret configured
- [ ] CORS allowed origins set to Blazor domain
- [ ] Logging configured appropriately
- [ ] Environment variable set to "Production" (in web.config)
- [ ] Swagger disabled (or access restricted)

### Blazor Configuration
- [ ] Update `appsettings.json` with API URL
- [ ] ApiBaseUrl points to: `https://api.yourdomain.com`
- [ ] No development settings remaining

## Phase 7: Database Setup

### Database Initialization
- [ ] Connection string tested
- [ ] Migrations executed (if needed)
- [ ] Seed data loaded (if applicable)
- [ ] Database schema verified
- [ ] Test queries executed successfully

### Run Migrations (if needed)
```powershell
cd C:\inetpub\wwwroot\FSH.Starter.API
- [ ] dotnet FSH.Starter.WebApi.Host.dll --migrate
```

## Phase 8: Testing

### API Testing
- [ ] Browse to API URL: `https://api.yourdomain.com`
- [ ] Swagger loads (if enabled) or health endpoint responds
- [ ] Test authentication endpoint
- [ ] Test a sample GET request
- [ ] Test a sample POST request
- [ ] Check application logs for errors
- [ ] Verify database connections working
- [ ] Test file upload functionality (if applicable)

### Blazor Testing
- [ ] Browse to Blazor URL: `https://app.yourdomain.com`
- [ ] Application loads without errors
- [ ] Check browser console (F12) for errors
- [ ] Test navigation between pages
- [ ] Test login functionality
- [ ] Test API integration
- [ ] Test responsive design (mobile/tablet)
- [ ] Verify all images and assets load

### Integration Testing
- [ ] User can register new account
- [ ] User can login
- [ ] User can access protected resources
- [ ] CRUD operations work correctly
- [ ] File uploads work (if applicable)
- [ ] Email sending works (if configured)
- [ ] Search functionality works
- [ ] Filters and sorting work

### Browser Compatibility
- [ ] Test in Chrome
- [ ] Test in Firefox
- [ ] Test in Edge
- [ ] Test in Safari (if targeting Mac users)
- [ ] Test on mobile browsers

## Phase 9: Security Verification

### SSL/TLS
- [ ] HTTPS working on API
- [ ] HTTPS working on Blazor
- [ ] HTTP redirects to HTTPS
- [ ] SSL certificate valid and not expired
- [ ] No mixed content warnings
- [ ] SSL Labs test passed (A rating or better)

### Security Headers
- [ ] X-Frame-Options header present
- [ ] X-Content-Type-Options header present
- [ ] X-XSS-Protection header present
- [ ] Content-Security-Policy configured (optional but recommended)

### Application Security
- [ ] JWT tokens working correctly
- [ ] Unauthorized access blocked
- [ ] CORS policy enforced
- [ ] SQL injection prevention verified
- [ ] XSS prevention verified
- [ ] File upload restrictions in place

## Phase 10: Performance & Monitoring

### Performance
- [ ] Application loads in reasonable time (< 3 seconds)
- [ ] API responses are fast (< 500ms for most requests)
- [ ] No memory leaks observed
- [ ] CPU usage normal
- [ ] Database query performance acceptable

### Logging & Monitoring
- [ ] Application logs being written
- [ ] IIS logs being written
- [ ] Log rotation configured
- [ ] Error notifications configured (optional)
- [ ] Performance counters accessible
- [ ] Uptime monitoring configured (optional)

### Monitoring Commands
```powershell
- [ ] Get-Content C:\inetpub\wwwroot\FSH.Starter.API\logs\stdout_*.log -Tail 10
- [ ] Get-WebAppPoolState -Name "FSH.Starter.API.Pool"
- [ ] Get-Counter '\Processor(_Total)\% Processor Time'
```

## Phase 11: Backup & Recovery

### Backup Setup
- [ ] Database backup schedule configured
- [ ] Application files backup configured
- [ ] Configuration files backed up
- [ ] IIS configuration exported
- [ ] Recovery procedure documented
- [ ] Backup restoration tested

### Backup Commands
```powershell
# Export IIS configuration
- [ ] Export-IISConfiguration -Path "C:\Backups\IIS-Config.zip"

# Backup application files
- [ ] Compress-Archive -Path "C:\inetpub\wwwroot\FSH.Starter.*" -Destination "C:\Backups\FSH-Apps.zip"
```

## Phase 12: Documentation

### Documentation Complete
- [ ] Deployment procedure documented
- [ ] Configuration settings documented
- [ ] Troubleshooting guide updated
- [ ] Server access credentials stored securely
- [ ] Database connection details documented
- [ ] SSL certificate renewal dates noted
- [ ] Support contacts documented

### Handover (if applicable)
- [ ] Operations team trained
- [ ] Access credentials provided
- [ ] Support procedures reviewed
- [ ] Escalation paths defined

## Phase 13: Post-Deployment

### Monitoring (First 24 Hours)
- [ ] Hour 1: Check logs and performance
- [ ] Hour 4: Verify application still running
- [ ] Hour 8: Check error rates
- [ ] Hour 24: Review logs, performance metrics

### First Week
- [ ] Day 1: Monitor closely, be ready for hotfixes
- [ ] Day 2: Review logs for any patterns
- [ ] Day 3: Check performance metrics
- [ ] Day 7: Full system health check

### Communication
- [ ] Stakeholders notified of successful deployment
- [ ] Users informed of new features (if applicable)
- [ ] Support team briefed on changes
- [ ] Documentation shared with relevant parties

## Phase 14: Optimization (Optional)

### Performance Tuning
- [ ] Response compression enabled
- [ ] Output caching configured
- [ ] CDN configured for static assets
- [ ] Database indexes optimized
- [ ] Query performance analyzed

### Advanced Configuration
- [ ] Rate limiting configured
- [ ] Application Insights configured
- [ ] Health checks configured
- [ ] Load balancing configured (if multiple servers)

## Rollback Plan

In case something goes wrong:
- [ ] Previous version backed up
- [ ] Rollback procedure documented
- [ ] Database rollback plan ready
- [ ] Quick switch to previous version possible

### Rollback Commands
```powershell
# Stop current version
Stop-WebAppPool -Name "FSH.Starter.API.Pool"

# Restore backup
Expand-Archive -Path "C:\Backups\FSH-Apps-Previous.zip" -Destination "C:\inetpub\wwwroot\" -Force

# Start previous version
Start-WebAppPool -Name "FSH.Starter.API.Pool"
```

---

## Completion Status

| Phase | Status | Date Completed | Notes |
|-------|--------|----------------|-------|
| 1. Pre-Deployment Preparation | ☐ | | |
| 2. Server Setup | ☐ | | |
| 3. Build & Package | ☐ | | |
| 4. File Transfer | ☐ | | |
| 5. IIS Configuration | ☐ | | |
| 6. Configuration Updates | ☐ | | |
| 7. Database Setup | ☐ | | |
| 8. Testing | ☐ | | |
| 9. Security Verification | ☐ | | |
| 10. Performance & Monitoring | ☐ | | |
| 11. Backup & Recovery | ☐ | | |
| 12. Documentation | ☐ | | |
| 13. Post-Deployment | ☐ | | |
| 14. Optimization | ☐ | | |

---

## Sign-off

**Deployed by:** _________________________ **Date:** _____________

**Verified by:** _________________________ **Date:** _____________

**Approved by:** _________________________ **Date:** _____________

---

**Notes:**
- Check off each item as you complete it
- Add notes for any issues encountered
- Keep this checklist for future deployments
- Update checklist based on lessons learned

Good luck with your deployment! 🚀


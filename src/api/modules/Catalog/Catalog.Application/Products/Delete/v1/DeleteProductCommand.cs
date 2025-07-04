﻿using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Delete.v1;
public sealed record DeleteProductCommand(
    DefaultIdType Id) : IRequest;

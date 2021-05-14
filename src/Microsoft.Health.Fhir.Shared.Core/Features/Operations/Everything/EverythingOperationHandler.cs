﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MediatR;
using Microsoft.Health.Core.Features.Security.Authorization;
using Microsoft.Health.Fhir.Core.Exceptions;
using Microsoft.Health.Fhir.Core.Features.Search;
using Microsoft.Health.Fhir.Core.Features.Security;
using Microsoft.Health.Fhir.Core.Messages.Everything;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Features.Operations.Everything
{
    public class EverythingOperationHandler : IRequestHandler<EverythingOperationRequest, EverythingOperationResponse>
    {
        private readonly IPatientEverythingService _patientEverythingService;
        private readonly IBundleFactory _bundleFactory;
        private readonly IAuthorizationService<DataActions> _authorizationService;

        public EverythingOperationHandler(IPatientEverythingService patientEverythingService, IBundleFactory bundleFactory, IAuthorizationService<DataActions> authorizationService)
        {
            EnsureArg.IsNotNull(patientEverythingService, nameof(patientEverythingService));
            EnsureArg.IsNotNull(bundleFactory, nameof(bundleFactory));
            EnsureArg.IsNotNull(authorizationService, nameof(authorizationService));

            _patientEverythingService = patientEverythingService;
            _bundleFactory = bundleFactory;
            _authorizationService = authorizationService;
        }

        public async Task<EverythingOperationResponse> Handle(EverythingOperationRequest request, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            if (await _authorizationService.CheckAccess(DataActions.Read, cancellationToken) != DataActions.Read)
            {
                throw new UnauthorizedFhirActionException();
            }

            SearchResult searchResult = await _patientEverythingService.SearchAsync(
                request.ResourceType, request.ResourceId, request.Start, request.End, request.Since, request.Type, request.Count, request.ContinuationToken, cancellationToken);

            ResourceElement bundle = _bundleFactory.CreateSearchBundle(searchResult);

            return new EverythingOperationResponse(bundle);
        }
    }
}

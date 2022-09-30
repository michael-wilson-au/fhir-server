﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Npgsql;

namespace Microsoft.Health.Fhir.Store.Sharding
{
    public class CitusService
    {
        private readonly string _connectionString;

        public CitusService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int MergeResources(
            IEnumerable<Resource> resources,
            IEnumerable<ReferenceSearchParam> referenceSearchParams,
            IEnumerable<TokenSearchParam> tokenSearchParams,
            IEnumerable<CompartmentAssignment> compartmentAssignments,
            IEnumerable<TokenText> tokenTexts,
            IEnumerable<DateTimeSearchParam> dateTimeSearchParams,
            IEnumerable<TokenQuantityCompositeSearchParam> tokenQuantityCompositeSearchParams,
            IEnumerable<QuantitySearchParam> quantitySearchParams,
            IEnumerable<StringSearchParam> stringSearchParams,
            IEnumerable<TokenTokenCompositeSearchParam> tokenTokenCompositeSearchParams,
            IEnumerable<TokenStringCompositeSearchParam> tokenStringCompositeSearchParams)
        {
            int c = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                c += connection.BulkLoadTable(resources, "resource");
                c += connection.BulkLoadTable(referenceSearchParams, "referencesearchparam");
                c += connection.BulkLoadTable(tokenSearchParams, "tokensearchparam");
                c += connection.BulkLoadTable(compartmentAssignments, "compartmentassignment");
                c += connection.BulkLoadTable(tokenTexts, "tokentext");
                c += connection.BulkLoadTable(dateTimeSearchParams, "datetimesearchparam");
                c += connection.BulkLoadTable(tokenQuantityCompositeSearchParams, "tokenquantitycompositesearchparam");
                c += connection.BulkLoadTable(quantitySearchParams, "quantitysearchparam");
                c += connection.BulkLoadTable(stringSearchParams, "stringsearchparam");
                c += connection.BulkLoadTable(tokenTokenCompositeSearchParams, "tokentokencompositesearchparam");
                c += connection.BulkLoadTable(tokenStringCompositeSearchParams, "tokenstringcompositesearchparam");
            }

            return c;
        }
    }
}
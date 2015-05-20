﻿//
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.DataFactories.Conversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.Management.DataFactories.Models
{
    public abstract class TypeProperties
    {
#if ADF_INTERNAL
        public IDictionary<string, JToken> ServiceExtraProperties { get; set; }
#endif

        protected TypeProperties()
        {
#if ADF_INTERNAL
            this.ServiceExtraProperties = new Dictionary<string, JToken>();
#endif
        }

        internal static TypeProperties DeserializeObject(string json, Type type)
        {
            return (TypeProperties)JsonConvert.DeserializeObject(
                    json,
                    type,
                    Converters());
        }

        internal string SerializeObject()
        {
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                new JsonSerializerSettings()
                    {
                        Converters = Converters(), 
                        NullValueHandling = NullValueHandling.Ignore, 
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
        }

        private static JsonConverter[] Converters()
        {
            return new JsonConverter[]
                       {
                           new TypePropertiesConverter(), new PolymorphicTypeConverter<StorageFormat>(),
                           new PolymorphicTypeConverter<PartitionValue>(), new PolymorphicTypeConverter<CopyLocation>(),
                           new PolymorphicTypeConverter<CopyTranslator>()
                       };
        }
    }
}
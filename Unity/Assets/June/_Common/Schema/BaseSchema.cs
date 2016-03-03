using System;
using System.Collections.Generic;


namespace June.Core.Schema {
    /// <summary>
    /// Basic fields that will be present in all records
    /// </summary>
    public class BaseSchema {
        /// <summary>
        /// _id
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// cts
        /// </summary>
        public const string CreatedTimeStamp = "cts";

        /// <summary>
        /// lts
        /// </summary>
        public const string LastModifiedTimeStamp = "lts";
    }
}
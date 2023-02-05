﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A PushChargingPoolAdminStatus result.
    /// </summary>
    public class PushChargingPoolAdminStatusResult
    {

        #region Properties

        /// <summary>
        /// The unqiue identification of the authenticator.
        /// </summary>
        public IId                                         AuthId                                     { get; }

        /// <summary>
        /// An object implementing ISendAdminStatus.
        /// </summary>
        public ISendAdminStatus?                           ISendAdminStatus                           { get; }

        /// <summary>
        /// An object implementing IReceiveAdminStatus.
        /// </summary>
        public IReceiveAdminStatus?                        IReceiveAdminStatus                        { get; }

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public PushChargingPoolAdminStatusResultTypes      Result                                     { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String?                                     Description                                { get; }

        /// <summary>
        /// An enumeration of rejected charging pool admin status updates.
        /// </summary>
        public IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<Warning>                        Warnings                                   { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                                   Runtime                                    { get;  }

        #endregion

        #region Constructor(s)

        #region (private)  PushChargingPoolAdminStatusResult(AuthId,                      Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected charging pool admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private PushChargingPoolAdminStatusResult(IId                                          AuthId,
                                                  PushChargingPoolAdminStatusResultTypes       Result,
                                                  String?                                      Description                              = null,
                                                  IEnumerable<ChargingPoolAdminStatusUpdate>?  RejectedChargingPoolAdminStatusUpdates   = null,
                                                  IEnumerable<Warning>?                        Warnings                                 = null,
                                                  TimeSpan?                                    Runtime                                  = null)
        {

            this.AuthId                                  = AuthId;
            this.Result                                  = Result;

            this.Description                             = Description is not null && Description.IsNotNullOrEmpty()
                                                               ? Description.Trim()
                                                               : String.Empty;

            this.RejectedChargingPoolAdminStatusUpdates  = RejectedChargingPoolAdminStatusUpdates ?? Array.Empty<ChargingPoolAdminStatusUpdate>();

            this.Warnings                                = Warnings is not null
                                                               ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                                               : Array.Empty<Warning>();

            this.Runtime                                 = Runtime;

        }

        #endregion

        #region (internal) PushChargingPoolAdminStatusResult(AuthId, ISendAdminStatus,    Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="ISendAdminStatus">An object implementing ISendAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected charging pool admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolAdminStatusResult(IId                                          AuthId,
                                                   ISendAdminStatus                             ISendAdminStatus,
                                                   PushChargingPoolAdminStatusResultTypes       Result,
                                                   String?                                      Description                              = null,
                                                   IEnumerable<ChargingPoolAdminStatusUpdate>?  RejectedChargingPoolAdminStatusUpdates   = null,
                                                   IEnumerable<Warning>?                        Warnings                                 = null,
                                                   TimeSpan?                                    Runtime                                  = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.ISendAdminStatus = ISendAdminStatus;

        }

        #endregion

        #region (internal) PushChargingPoolAdminStatusResult(AuthId, IReceiveAdminStatus, Result, ...)

        /// <summary>
        /// Create a new PushChargingPoolAdminStatus result.
        /// </summary>
        /// <param name="AuthId">The unqiue identification of the authenticator.</param>
        /// <param name="IReceiveAdminStatus">An object implementing IReceiveAdminStatus.</param>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="RejectedChargingPoolAdminStatusUpdates">An enumeration of rejected charging pool admin status updates.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        internal PushChargingPoolAdminStatusResult(IId                                          AuthId,
                                                   IReceiveAdminStatus                          IReceiveAdminStatus,
                                                   PushChargingPoolAdminStatusResultTypes       Result,
                                                   String?                                      Description                              = null,
                                                   IEnumerable<ChargingPoolAdminStatusUpdate>?  RejectedChargingPoolAdminStatusUpdates   = null,
                                                   IEnumerable<Warning>?                        Warnings                                 = null,
                                                   TimeSpan?                                    Runtime                                  = null)

            : this(AuthId,
                   Result,
                   Description,
                   RejectedChargingPoolAdminStatusUpdates,
                   Warnings,
                   Runtime)

        {

            this.IReceiveAdminStatus = IReceiveAdminStatus;

        }

        #endregion

        #endregion


        #region Success

        public static PushChargingPoolAdminStatusResult

            Success(IId                    AuthId,
                    ISendAdminStatus       ISendAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingPoolAdminStatusResult

            Success(IId                    AuthId,
                    IReceiveAdminStatus    IReceiveAdminStatus,
                    String?                Description   = null,
                    IEnumerable<Warning>?  Warnings      = null,
                    TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.Success,
                    Description,
                    Array.Empty<ChargingPoolAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion


        #region Enqueued

        public static PushChargingPoolAdminStatusResult

            Enqueued(IId                    AuthId,
                     ISendAdminStatus       ISendAdminStatus,
                     String?                Description   = null,
                     IEnumerable<Warning>?  Warnings      = null,
                     TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.Enqueued,
                    Description,
                    Array.Empty<ChargingPoolAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region NoOperation

        public static PushChargingPoolAdminStatusResult

            NoOperation(IId                    AuthId,
                        ISendAdminStatus       ISendAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingPoolAdminStatusUpdate>(),
                    Warnings,
                    Runtime);



        public static PushChargingPoolAdminStatusResult

            NoOperation(IId                    AuthId,
                        IReceiveAdminStatus    IReceiveAdminStatus,
                        String?                Description   = null,
                        IEnumerable<Warning>?  Warnings      = null,
                        TimeSpan?              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.NoOperation,
                    Description,
                    Array.Empty<ChargingPoolAdminStatusUpdate>(),
                    Warnings,
                    Runtime);

        #endregion

        #region OutOfService

        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         ISendAdminStatus                                       ISendAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingPoolAdminStatusResult

            OutOfService(IId                                                    AuthId,
                         IReceiveAdminStatus                                    IReceiveAdminStatus,
                         IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                         String?                                                Description   = null,
                         IEnumerable<Warning>?                                  Warnings      = null,
                         TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.OutOfService,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region AdminDown

        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      ISendAdminStatus                                       ISendAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);



        public static PushChargingPoolAdminStatusResult

            AdminDown(IId                                                    AuthId,
                      IReceiveAdminStatus                                    IReceiveAdminStatus,
                      IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                      String?                                                Description   = null,
                      IEnumerable<Warning>?                                  Warnings      = null,
                      TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.AdminDown,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion


        #region Error

        public static PushChargingPoolAdminStatusResult

            Error(IId                                                     AuthId,
                  ISendAdminStatus                                        ISendAdminStatus,
                  IEnumerable<ChargingPoolAdminStatusUpdate>?  RejectedChargingPoolAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);


        public static PushChargingPoolAdminStatusResult

            Error(IId                                                     AuthId,
                  IReceiveAdminStatus                                     IReceiveAdminStatus,
                  IEnumerable<ChargingPoolAdminStatusUpdate>?  RejectedChargingPoolAdminStatusUpdates   = null,
                  String?                                                 Description                                         = null,
                  IEnumerable<Warning>?                                   Warnings                                            = null,
                  TimeSpan?                                               Runtime                                             = null)

            => new (AuthId,
                    IReceiveAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.Error,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion

        #region LockTimeout

        public static PushChargingPoolAdminStatusResult

            LockTimeout(IId                                                    AuthId,
                        ISendAdminStatus                                       ISendAdminStatus,
                        IEnumerable<ChargingPoolAdminStatusUpdate>  RejectedChargingPoolAdminStatusUpdates,
                        String?                                                Description   = null,
                        IEnumerable<Warning>?                                  Warnings      = null,
                        TimeSpan?                                              Runtime       = null)

            => new (AuthId,
                    ISendAdminStatus,
                    PushChargingPoolAdminStatusResultTypes.LockTimeout,
                    Description,
                    RejectedChargingPoolAdminStatusUpdates,
                    Warnings,
                    Runtime);

        #endregion



        #region Flatten(AuthId, ISendAdminStatus, PushChargingPoolAdminStatusResults, Runtime)

        public static PushChargingPoolAdminStatusResult Flatten(IId                                                        AuthId,
                                                                           ISendAdminStatus                                           ISendAdminStatus,
                                                                           IEnumerable<PushChargingPoolAdminStatusResult>  PushChargingPoolAdminStatusResults,
                                                                           TimeSpan                                                   Runtime)
        {

            #region Initial checks

            if (PushChargingPoolAdminStatusResults is null || !PushChargingPoolAdminStatusResults.Any())
                return new PushChargingPoolAdminStatusResult(AuthId,
                                                                        ISendAdminStatus,
                                                                        PushChargingPoolAdminStatusResultTypes.Error,
                                                                        "!",
                                                                        Array.Empty<ChargingPoolAdminStatusUpdate>(),
                                                                        Array.Empty<Warning>(),
                                                                        Runtime);

            #endregion

            var all                                                = PushChargingPoolAdminStatusResults.ToArray();

            var resultOverview                                     = all.GroupBy      (result => result.Result).
                                                                         ToDictionary (result => result.Key,
                                                                                       result => new List<PushChargingPoolAdminStatusResult>(result));

            var descriptions                                       = all.Where        (result => result is not null).
                                                                         SafeSelect   (result => result.Description).
                                                                         AggregateWith(Environment.NewLine);

            var rejectedChargingPoolAdminStatusUpdates  = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.RejectedChargingPoolAdminStatusUpdates);

            var warnings                                           = all.Where        (result => result is not null).
                                                                         SelectMany   (result => result.Warnings);


            foreach (var result in resultOverview)
                if (resultOverview[result.Key].Count == all.Length)
                    return new PushChargingPoolAdminStatusResult(all[0].AuthId,
                                                                            ISendAdminStatus,
                                                                            result.Key,
                                                                            descriptions,
                                                                            rejectedChargingPoolAdminStatusUpdates,
                                                                            warnings,
                                                                            Runtime);

            return new PushChargingPoolAdminStatusResult(all[0].AuthId,
                                                                    ISendAdminStatus,
                                                                    PushChargingPoolAdminStatusResultTypes.Partial,
                                                                    descriptions,
                                                                    rejectedChargingPoolAdminStatusUpdates,
                                                                    warnings,
                                                                    Runtime);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }


    public enum PushChargingPoolAdminStatusResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The service was disabled by the administrator.
        /// </summary>
        AdminDown,

        Success,
        Partial,
        OutOfService,
        Error,
        True,
        NoOperation,
        Enqueued,
        LockTimeout,
        False

    }

}

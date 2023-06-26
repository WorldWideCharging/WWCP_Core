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

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    public class EMobilityProviderAPIClient : AHTTPClient
    {

        /// <summary>
        /// Indicate a remote start of the given charging session at the given EVSE
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="LogTimestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the request.</param>
        public delegate Task OnRemoteStartRequestDelegate(DateTime            LogTimestamp,
                                                          Object              Sender,
                                                          RemoteStartRequest  Request);


        /// <summary>
        /// Indicate a remote start of the given charging session at the given EVSE
        /// and for the given provider/eMAId.
        /// </summary>
        /// <param name="LogTimestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the request.</param>
        public delegate Task OnRemoteStartResponseDelegate(DateTime             LogTimestamp,
                                                           Object               Sender,
                                                           RemoteStartRequest   Request,
                                                           RemoteStartResponse  Response,
                                                           TimeSpan             Runtime);


        /// <summary>
        /// Indicate a remote stop of the given charging session.
        /// </summary>
        /// <param name="LogTimestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the request.</param>
        public delegate Task OnRemoteStopRequestDelegate(DateTime           LogTimestamp,
                                                         Object             Sender,
                                                         RemoteStopRequest  Request);


        /// <summary>
        /// Indicate a remote stop of the given charging session.
        /// </summary>
        /// <param name="LogTimestamp">The timestamp of the request.</param>
        /// <param name="Sender">The sender of the request.</param>
        public delegate Task OnRemoteStopResponseDelegate(DateTime            LogTimestamp,
                                                          Object              Sender,
                                                          RemoteStopRequest   Request,
                                                          RemoteStopResponse  Response,
                                                          TimeSpan            Runtime);


        #region (class) CommonAPICounters

        public class APICounters
        {

            public APICounterValues  RemoteStart             { get; }
            public APICounterValues  RemoteStop              { get; }

            public APICounters(APICounterValues?  RemoteStart   = null,
                               APICounterValues?  RemoteStop    = null)
            {

                this.RemoteStart  = RemoteStart ?? new APICounterValues();
                this.RemoteStop   = RemoteStop  ?? new APICounterValues();

            }

            public virtual JObject ToJSON()

                => JSONObject.Create(
                       new JProperty("remoteStart",  RemoteStart.ToJSON()),
                       new JProperty("remoteStop",   RemoteStop. ToJSON())
                   );

        }

        #endregion

        #region Data

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public new const        String    DefaultHTTPUserAgent        = $"GraphDefined E-Mobility Provider HTTP Client";

        /// <summary>
        /// The default timeout for HTTP requests.
        /// </summary>
        public new readonly     TimeSpan  DefaultRequestTimeout       = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The default maximum number of transmission retries for HTTP request.
        /// </summary>
        public new const        UInt16    DefaultMaxNumberOfRetries   = 3;

        #endregion

        #region Properties

//        /// <summary>
//        /// The attached HTTP client logger.
//        /// </summary>
//        public new HTTP_Logger             HTTPLogger
//#pragma warning disable CS8603 // Possible null reference return.
//            => base.HTTPLogger as HTTP_Logger;
//#pragma warning restore CS8603 // Possible null reference return.

//        /// <summary>
//        /// The attached client logger.
//        /// </summary>
//        public CPOClientLogger?            Logger            { get; }

        public APICounters                 Counters          { get; }

        public Newtonsoft.Json.Formatting  JSONFormatting    { get; set; }

        #endregion

        #region Custom JSON parsers

        public CustomJObjectParserDelegate<RemoteStartResponse>?  CustomRemoteStartResponseParser    { get; set; }

        public CustomJObjectParserDelegate<RemoteStopResponse>?   CustomRemoteStopResponseParser     { get; set; }

        #endregion

        #region Custom JSON serializers

        public CustomJObjectSerializerDelegate<RemoteStartRequest>?  CustomRemoteStartRequestSerializer    { get; set; }
        public CustomJObjectSerializerDelegate<RemoteStopRequest>?   CustomRemoteStopRequestSerializer     { get; set; }

        #endregion

        #region Events

        #region OnRemoteReservationStartRequest/-Response

        ///// <summary>
        ///// An event fired whenever an RemoteReservationReservationStart request will be send.
        ///// </summary>
        //public event OnRemoteReservationStartRequestDelegate?   OnRemoteReservationStartRequest;

        ///// <summary>
        ///// An event fired whenever an RemoteReservationReservationStart HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler?                            OnRemoteReservationStartHTTPRequest;

        ///// <summary>
        ///// An event fired whenever a response for an RemoteReservationReservationStart HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler?                           OnRemoteReservationStartHTTPResponse;

        ///// <summary>
        ///// An event fired whenever a response for an RemoteReservationReservationStart request had been received.
        ///// </summary>
        //public event OnRemoteReservationStartResponseDelegate?  OnRemoteReservationStartResponse;

        #endregion

        #region OnRemoteReservationStopRequest/-Response

        ///// <summary>
        ///// An event fired whenever an RemoteReservationReservationStop request will be send.
        ///// </summary>
        //public event OnRemoteReservationStopRequestDelegate?   OnRemoteReservationStopRequest;

        ///// <summary>
        ///// An event fired whenever an RemoteReservationReservationStop HTTP request will be send.
        ///// </summary>
        //public event ClientRequestLogHandler?                           OnRemoteReservationStopHTTPRequest;

        ///// <summary>
        ///// An event fired whenever a response for an RemoteReservationReservationStop HTTP request had been received.
        ///// </summary>
        //public event ClientResponseLogHandler?                          OnRemoteReservationStopHTTPResponse;

        ///// <summary>
        ///// An event fired whenever a response for an RemoteReservationReservationStop request had been received.
        ///// </summary>
        //public event OnRemoteReservationStopResponseDelegate?  OnRemoteReservationStopResponse;

        #endregion


        #region OnRemoteStartRequest/-Response

        /// <summary>
        /// An event fired whenever an RemoteStart request will be send.
        /// </summary>
        public event OnRemoteStartRequestDelegate?    OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever an RemoteStart HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?         OnRemoteStartHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an RemoteStart HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?        OnRemoteStartHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an RemoteStart request had been received.
        /// </summary>
        public event OnRemoteStartResponseDelegate?   OnRemoteStartResponse;

        #endregion

        #region OnRemoteStopRequest/-Response

        /// <summary>
        /// An event fired whenever an RemoteStop request will be send.
        /// </summary>
        public event OnRemoteStopRequestDelegate?    OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever an RemoteStop HTTP request will be send.
        /// </summary>
        public event ClientRequestLogHandler?        OnRemoteStopHTTPRequest;

        /// <summary>
        /// An event fired whenever a response for an RemoteStop HTTP request had been received.
        /// </summary>
        public event ClientResponseLogHandler?       OnRemoteStopHTTPResponse;

        /// <summary>
        /// An event fired whenever a response for an RemoteStop request had been received.
        /// </summary>
        public event OnRemoteStopResponseDelegate?   OnRemoteStopResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public EMobilityProviderAPIClient(URL                                   RemoteURL,
                                          HTTPHostname?                         VirtualHostname              = null,
                                          String?                               Description                  = null,
                                          RemoteCertificateValidationCallback?  RemoteCertificateValidator   = null,
                                          LocalCertificateSelectionCallback?    ClientCertificateSelector    = null,
                                          X509Certificate?                      ClientCert                   = null,
                                          SslProtocols?                         TLSProtocol                  = null,
                                          Boolean?                              PreferIPv4                   = null,
                                          String                                HTTPUserAgent                = DefaultHTTPUserAgent,
                                          TimeSpan?                             RequestTimeout               = null,
                                          TransmissionRetryDelayDelegate?       TransmissionRetryDelay       = null,
                                          UInt16?                               MaxNumberOfRetries           = DefaultMaxNumberOfRetries,
                                          Boolean?                              DisableLogging               = false,
                                          String?                               LoggingPath                  = null,
                                          String                                LoggingContext               = "",//EMobilityProviderAPIClientLogger.DefaultContext,
                                          LogfileCreatorDelegate?               LogfileCreator               = null,
                                          DNSClient?                            DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   PreferIPv4,
                   HTTPUserAgent       ?? DefaultHTTPUserAgent,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries  ?? DefaultMaxNumberOfRetries,
                   false,
                   DisableLogging,
                   null,
                   DNSClient)

        {

            this.Counters        = new APICounters();

            this.JSONFormatting  = Newtonsoft.Json.Formatting.None;

            //base.HTTPLogger      = this.DisableLogging == false
            //                           ? new HTTP_Logger(this,
            //                                             LoggingPath,
            //                                             LoggingContext,
            //                                             LogfileCreator)
            //                           : null;

            //this.Logger          = this.DisableLogging == false
            //                           ? new EMobilityProviderAPIClientLogger(this,
            //                                                 LoggingPath,
            //                                                 LoggingContext,
            //                                                 LogfileCreator)
            //                           : null;

        }

        #endregion


        #region RemoteStart(Request)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Request">An RemoteStart request.</param>
        public async Task<RemoteStartResponse>

            RemoteStart(RemoteStartRequest Request)

        {

            #region Initial checks

            //Request = _CustomRemoteStartRequestMapper(Request);

            Byte                  transmissionRetry   = 0;
            RemoteStartResponse?  response              = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            Counters.RemoteStart.IncRequests_OK();

            try
            {

                if (OnRemoteStartRequest is not null)
                    await Task.WhenAll(OnRemoteStartRequest.GetInvocationList().
                                       Cast<OnRemoteStartRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProviderAPIClient) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + "remoteStart",
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomRemoteStartRequestSerializer).
                                                                                                                             ToString(JSONFormatting).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnRemoteStartHTTPRequest,
                                                      ResponseLogDelegate:  OnRemoteStartHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (RemoteStartResponse.TryParse(Request,
                                                                 JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                 HTTPResponse.Timestamp,
                                                                 HTTPResponse.Runtime,
                                                                 out response,
                                                                 out var errorResponse,
                                                                 HTTPResponse,
                                                                 CustomRemoteStartResponseParser) &&
                                    response is not null)
                                {

                                    Counters.RemoteStart.IncResponses_OK();

                                }

                            }
                            catch (Exception e)
                            {

                                response = new RemoteStartResponse(
                                               Request,
                                               RemoteStartResultTypes.Error,
                                               HTTPResponse.EventTrackingId,
                                               HTTPResponse.Timestamp,
                                               null,
                                               I18NString.Create(Languages.en, e.Message),
                                               e.StackTrace,
                                               Array.Empty<Warning>(),
                                               null, // CustomData
                                               HTTPResponse.Runtime,
                                               HTTPResponse
                                           );

                            }

                        }

                        transmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            //if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                            //                                 out var validationErrorList,
                            //                                 out var errorResponse))
                            //{

                            response = new RemoteStartResponse(Request,
                                                               RemoteStartResultTypes.BadRequest,
                                                               HTTPResponse.EventTrackingId,
                                                               HTTPResponse.Timestamp,
                                                               Runtime:      HTTPResponse.Runtime,
                                                               HTTPResponse: HTTPResponse);

                            //}

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        //if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                        //    HTTPResponse.HTTPBody.Length > 0)
                        //{

                        //    try
                        //    {

                        //        var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                        //        if (json is not null &&
                        //            json["StatusCode"] is JObject JSONObject &&
                        //            StatusCode.TryParse(JSONObject,
                        //                                out StatusCode? statusCode,
                        //                                out String? ErrorResponse,
                        //                                CustomStatusCodeParser))
                        //        {

                        response = new RemoteStartResponse(Request,
                                                         RemoteStartResultTypes.Unauthorized,
                                                         HTTPResponse.EventTrackingId,
                                                         HTTPResponse.Timestamp,
                                                         Runtime:      HTTPResponse.Runtime,
                                                         HTTPResponse: HTTPResponse);

                        //            result = RemoteStartResponse.Failed(Request,
                        //                                                                                     new Acknowledgement<RemoteStartRequest>(
                        //                                                                                         HTTPResponse.Timestamp,
                        //                                                                                         HTTPResponse.EventTrackingId,
                        //                                                                                         processId,
                        //                                                                                         HTTPResponse.Runtime,
                        //                                                                                         statusCode!,
                        //                                                                                         Request,
                        //                                                                                         HTTPResponse,
                        //                                                                                         false,
                        //                                                                                         Request.SessionId,
                        //                                                                                         Request.CPOPartnerSessionId,
                        //                                                                                         Request.EMPPartnerSessionId,
                        //                                                                                         Request.CustomData
                        //                                                                                     ),
                        //                                                                                     processId);

                        //        }

                        //    }
                        //    catch (Exception e)
                        //    {

                        //        result = RemoteStartResponse.Failed(
                        //                     Request,
                        //                     new Acknowledgement<RemoteStartRequest>(
                        //                         HTTPResponse.Timestamp,
                        //                         HTTPResponse.EventTrackingId,
                        //                         processId,
                        //                         HTTPResponse.Runtime,
                        //                         new StatusCode(
                        //                             StatusCodes.SystemError,
                        //                             e.Message,
                        //                             e.StackTrace
                        //                         ),
                        //                         Request,
                        //                         HTTPResponse,
                        //                         false,
                        //                         Request.SessionId,
                        //                         Request.CPOPartnerSessionId,
                        //                         Request.EMPPartnerSessionId,
                        //                         Request.CustomData
                        //                     )
                        //                 );

                        //    }

                        //}

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (transmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                response = new RemoteStartResponse(
                               Request,
                               RemoteStartResultTypes.Error,
                               Request.EventTrackingId,
                               Timestamp.Now,
                               null, // ChargingSession
                               I18NString.Create(Languages.en, e.Message),
                               e.StackTrace,
                               Array.Empty<Warning>(),
                               null, // CustomData
                               Timestamp.Now - startTime,
                               null  // HTTPResponse
                           );

            }

            response ??= new RemoteStartResponse(
                             Request,
                             RemoteStartResultTypes.Error,
                             Request.EventTrackingId,
                             Timestamp.Now,
                             null, // ChargingSession
                             I18NString.Create(Languages.en, "HTTP request failed!"),
                             null, // AdditionalInfo
                             Array.Empty<Warning>(),
                             null, // CustomData
                             Timestamp.Now - startTime,
                             null  // HTTPResponse
                         );

            if (response.Result != RemoteStartResultTypes.Success &&
                response.Result != RemoteStartResultTypes.AsyncOperation)
            {
                Counters.RemoteStart.IncResponses_Error();
            }


            #region Send OnRemoteStartResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnRemoteStartResponse is not null)
                    await Task.WhenAll(OnRemoteStartResponse.GetInvocationList().
                                       Cast<OnRemoteStartResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     response,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProviderAPIClient) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return response;

        }

        #endregion

        #region RemoteStop (Request)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="Request">An RemoteStop request.</param>
        public async Task<RemoteStopResponse>

            RemoteStop(RemoteStopRequest Request)

        {

            #region Initial checks

            //Request = _CustomRemoteStopRequestMapper(Request);

            Byte                 transmissionRetry   = 0;
            RemoteStopResponse?  response            = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = Timestamp.Now;

            Counters.RemoteStop.IncRequests_OK();

            try
            {

                if (OnRemoteStopRequest is not null)
                    await Task.WhenAll(OnRemoteStopRequest.GetInvocationList().
                                       Cast<OnRemoteStopRequestDelegate>().
                                       Select(e => e(startTime,
                                                     this,
                                                     Request))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProviderAPIClient) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                do
                {

                    #region Upstream HTTP request...

                    var HTTPResponse = await HTTPClientFactory.Create(RemoteURL,
                                                                      VirtualHostname,
                                                                      Description,
                                                                      RemoteCertificateValidator,
                                                                      ClientCertificateSelector,
                                                                      ClientCert,
                                                                      TLSProtocol,
                                                                      PreferIPv4,
                                                                      HTTPUserAgent,
                                                                      RequestTimeout,
                                                                      TransmissionRetryDelay,
                                                                      MaxNumberOfRetries,
                                                                      UseHTTPPipelining,
                                                                      DisableLogging,
                                                                      null,
                                                                      DNSClient).

                                              Execute(client => client.POSTRequest(RemoteURL.Path + "remoteStop",
                                                                                   requestbuilder => {
                                                                                       requestbuilder.Accept.Add(HTTPContentType.JSON_UTF8);
                                                                                       requestbuilder.ContentType  = HTTPContentType.JSON_UTF8;
                                                                                       requestbuilder.Content      = Request.ToJSON(CustomRemoteStopRequestSerializer).
                                                                                                                             ToString(JSONFormatting).
                                                                                                                             ToUTF8Bytes();
                                                                                       requestbuilder.Connection   = "close";
                                                                                   }),

                                                      RequestLogDelegate:   OnRemoteStopHTTPRequest,
                                                      ResponseLogDelegate:  OnRemoteStopHTTPResponse,
                                                      CancellationToken:    Request.CancellationToken,
                                                      EventTrackingId:      Request.EventTrackingId,
                                                      RequestTimeout:       Request.RequestTimeout ?? RequestTimeout).

                                              ConfigureAwait(false);

                    #endregion


                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.OK)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            try
                            {

                                if (RemoteStopResponse.TryParse(Request,
                                                                JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String()),
                                                                HTTPResponse.Timestamp,
                                                                HTTPResponse.Runtime,
                                                                out response,
                                                                out var errorResponse,
                                                                HTTPResponse,
                                                                CustomRemoteStopResponseParser) &&
                                    response is not null)
                                {

                                    Counters.RemoteStop.IncResponses_OK();

                                }

                            }
                            catch (Exception e)
                            {

                                response = new RemoteStopResponse(
                                               Request,
                                               RemoteStopResultTypes.Error,
                                               HTTPResponse.EventTrackingId,
                                               HTTPResponse.Timestamp,
                                               Request.ChargingSessionId,
                                               null,
                                               null,
                                               null,
                                               null,
                                               I18NString.Create(Languages.en, e.Message),
                                               e.StackTrace,
                                               Array.Empty<Warning>(),
                                               null, // CustomData
                                               HTTPResponse.Runtime,
                                               HTTPResponse
                                           );

                            }

                        }

                        transmissionRetry = Byte.MaxValue - 1;
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.BadRequest)
                    {

                        if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                            HTTPResponse.HTTPBody.Length > 0)
                        {

                            // HTTP/1.1 400 BadRequest
                            // Server:             nginx/1.18.0
                            // Date:               Fri, 08 Jan 2021 14:19:25 GMT
                            // Content-Type:       application/json;charset=utf-8
                            // Transfer-Encoding:  chunked
                            // Connection:         keep-alive
                            // Process-ID:         b87fd67b-2d74-4318-86cf-0d2c2c50cabb
                            // 
                            // {
                            //     "extendedInfo":  null,
                            //     "message":      "Error parsing/validating JSON.",
                            //     "validationErrors": [
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].hotlinePhoneNumber",
                            //             "errorMessage":   "must match \"^\\+[0-9]{5,15}$\""
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].geoCoordinates",
                            //             "errorMessage":   "may not be null"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].chargingStationNames",
                            //             "errorMessage":   "may not be empty"
                            //         },
                            //         {
                            //             "fieldReference": "operatorEvseData.evseDataRecord[0].plugs",
                            //             "errorMessage":   "may not be empty"
                            //         }
                            //     ]
                            // }

                            //if (ValidationErrorList.TryParse(JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String() ?? ""),
                            //                                 out var validationErrorList,
                            //                                 out var errorResponse))
                            //{

                                response = new RemoteStopResponse(Request,
                                                                  RemoteStopResultTypes.BadRequest,
                                                                  HTTPResponse.EventTrackingId,
                                                                  HTTPResponse.Timestamp,
                                                                  Request.ChargingSessionId,
                                                                  Runtime:      HTTPResponse.Runtime,
                                                                  HTTPResponse: HTTPResponse);

                            //}

                        }

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Forbidden)
                    {

                        // Hubject firewall problem!
                        // Only HTML response!
                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.Unauthorized)
                    {

                        // HTTP/1.1 401 Unauthorized
                        // Server:          nginx/1.18.0 (Ubuntu)
                        // Date:            Tue, 02 Mar 2021 23:09:35 GMT
                        // Content-Type:    application/json;charset=UTF-8
                        // Content-Length:  87
                        // Connection:      keep-alive
                        // Process-ID:      cefd3dfc-8807-4160-8913-d3153dfea8ab
                        // 
                        // {
                        //     "StatusCode": {
                        //         "Code":            "017",
                        //         "Description":     "Unauthorized Access",
                        //         "AdditionalInfo":   null
                        //     }
                        // }

                        // Operator/provider identification is not linked to the TLS client certificate!

                        //if (HTTPResponse.ContentType == HTTPContentType.JSON_UTF8 &&
                        //    HTTPResponse.HTTPBody.Length > 0)
                        //{

                        //    try
                        //    {

                        //        var json = JObject.Parse(HTTPResponse.HTTPBody.ToUTF8String());

                        //        if (json is not null &&
                        //            json["StatusCode"] is JObject JSONObject &&
                        //            StatusCode.TryParse(JSONObject,
                        //                                out StatusCode? statusCode,
                        //                                out String? ErrorResponse,
                        //                                CustomStatusCodeParser))
                        //        {

                        response = new RemoteStopResponse(Request,
                                                          RemoteStopResultTypes.Unauthorized,
                                                          HTTPResponse.EventTrackingId,
                                                          HTTPResponse.Timestamp,
                                                          Request.ChargingSessionId,
                                                          Runtime:      HTTPResponse.Runtime,
                                                          HTTPResponse: HTTPResponse);

                        //            response = OICPResult<Acknowledgement<RemoteStopRequest>>.Failed(Request,
                        //                                                                                    new Acknowledgement<RemoteStopRequest>(
                        //                                                                                        HTTPResponse.Timestamp,
                        //                                                                                        HTTPResponse.EventTrackingId,
                        //                                                                                        processId,
                        //                                                                                        HTTPResponse.Runtime,
                        //                                                                                        statusCode!,
                        //                                                                                        Request,
                        //                                                                                        HTTPResponse,
                        //                                                                                        false,
                        //                                                                                        Request.SessionId,
                        //                                                                                        Request.CPOPartnerSessionId,
                        //                                                                                        Request.EMPPartnerSessionId,
                        //                                                                                        Request.CustomData
                        //                                                                                    ),
                        //                                                                                    processId);

                        //        }

                        //    }
                        //    catch (Exception e)
                        //    {

                        //        response = OICPResult<Acknowledgement<RemoteStopRequest>>.Failed(
                        //                     Request,
                        //                     new Acknowledgement<RemoteStopRequest>(
                        //                         HTTPResponse.Timestamp,
                        //                         HTTPResponse.EventTrackingId,
                        //                         processId,
                        //                         HTTPResponse.Runtime,
                        //                         new StatusCode(
                        //                             StatusCodes.SystemError,
                        //                             e.Message,
                        //                             e.StackTrace
                        //                         ),
                        //                         Request,
                        //                         HTTPResponse,
                        //                         false,
                        //                         Request.SessionId,
                        //                         Request.CPOPartnerSessionId,
                        //                         Request.EMPPartnerSessionId,
                        //                         Request.CustomData
                        //                     )
                        //                 );

                        //    }

                        //}

                        break;

                    }

                    if (HTTPResponse.HTTPStatusCode == HTTPStatusCode.RequestTimeout)
                    { }

                }
                while (transmissionRetry++ < MaxNumberOfRetries);

            }
            catch (Exception e)
            {

                response = new RemoteStopResponse(
                               Request,
                               RemoteStopResultTypes.Error,
                               Request.EventTrackingId,
                               Timestamp.Now,
                               Request.ChargingSessionId,
                               null, // ChargingSession
                               null,
                               null,
                               null,
                               I18NString.Create(Languages.en, e.Message),
                               e.StackTrace,
                               Array.Empty<Warning>(),
                               null, // CustomData
                               Timestamp.Now - startTime,
                               null  // HTTPResponse
                           );

            }

            response ??= new RemoteStopResponse(
                             Request,
                             RemoteStopResultTypes.Error,
                             Request.EventTrackingId,
                             Timestamp.Now,
                             Request.ChargingSessionId,
                             null, // ChargingSession
                             null,
                             null,
                             null,
                             I18NString.Create(Languages.en, "HTTP request failed!"),
                             null, // AdditionalInfo
                             Array.Empty<Warning>(),
                             null, // CustomData
                             Timestamp.Now - startTime,
                             null  // HTTPResponse
                         );

            if (response.Result != RemoteStopResultTypes.Success &&
                response.Result != RemoteStopResultTypes.AsyncOperation)
            {
                Counters.RemoteStart.IncResponses_Error();
            }


            #region Send OnRemoteStopResponse event

            var endtime = Timestamp.Now;

            try
            {

                if (OnRemoteStopResponse is not null)
                    await Task.WhenAll(OnRemoteStopResponse.GetInvocationList().
                                       Cast<OnRemoteStopResponseDelegate>().
                                       Select(e => e(endtime,
                                                     this,
                                                     Request,
                                                     response,
                                                     endtime - startTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProviderAPIClient) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return response;

        }

        #endregion



    }

}

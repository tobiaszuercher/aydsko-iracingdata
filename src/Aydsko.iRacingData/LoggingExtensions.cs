﻿// © 2023 Adrian Clark
// This file is licensed to you under the MIT license.

using System.Net;

namespace Aydsko.iRacingData;

public static class LoggingExtensions
{
    private const int EventIdLoginSuccessful = 1;
    private const int EventIdRateLimitsUpdated = 2;
    private const int EventIdErrorResponseUnknownTrace = 3;
    private const int EventIdErrorResponseTrace = 4;
    private const int EventIdFailedToRetrieveChunkError = 5;
    private const int EventIdTraceCacheHitOrMiss = 6;

    private static readonly Action<ILogger, string, Exception?> loginSuccessful = LoggerMessage.Define<string>(LogLevel.Information,
                                                                                                               new EventId(EventIdLoginSuccessful, nameof(LoginSuccessful)),
                                                                                                               "Authenticated successfully as {UserEmail}");

    private static readonly Action<ILogger, int?, int?, DateTimeOffset?, Exception?> rateLimitsUpdated = LoggerMessage.Define<int?, int?, DateTimeOffset?>(LogLevel.Debug,
                                                                                                                                                           new EventId(EventIdRateLimitsUpdated, nameof(RateLimitsUpdated)),
                                                                                                                                                           "Currently have {RateLimitRemaining} calls left from {RateLimitTotal} resetting at {RateLimitResetInstant}");

    private static readonly Action<ILogger, Exception?> errorResponseUnknownTrace = LoggerMessage.Define(LogLevel.Trace,
                                                                                                         new EventId(EventIdErrorResponseUnknownTrace, nameof(ErrorResponseUnknown)),
                                                                                                         "Error response was unknown so throwing a standard HTTP error");

    private static readonly Action<ILogger, string?, Exception?> errorResponse = LoggerMessage.Define<string?>(LogLevel.Trace,
                                                                                                               new EventId(EventIdErrorResponseTrace, nameof(ErrorResponse)),
                                                                                                               "Error response from iRacing API: {ErrorDescription}");

    private static readonly Action<ILogger, int?, int?, HttpStatusCode?, string?, Exception?> failedToRetrieveChunkError = LoggerMessage.Define<int?, int?, HttpStatusCode?, string?>(LogLevel.Error,
                                                                                                                                                                                      new EventId(EventIdFailedToRetrieveChunkError, nameof(FailedToRetrieveChunkError)),
                                                                                                                                                                                      "Failed to retrieve chunk index {ChunkIndex} of {ChunkTotalCount} due to status code {HttpStatusCode} reason {HttpStatusReasonPhrase}");

    private static readonly Action<ILogger, Uri, bool, Exception?> traceCacheHitOrMiss = LoggerMessage.Define<Uri, bool>(LogLevel.Trace, EventIdTraceCacheHitOrMiss, "Cache status for {Url} is {HitStatus}");

    public static void LoginSuccessful(this ILogger logger, string userEmail)
    {
        loginSuccessful(logger, userEmail, null);
    }

    public static void RateLimitsUpdated(this ILogger logger, int? rateLimitRemaining, int? rateLimit, DateTimeOffset? rateLimitResetInstant)
    {
        rateLimitsUpdated(logger, rateLimitRemaining, rateLimit, rateLimitResetInstant, null);
    }

    public static void ErrorResponseUnknown(this ILogger logger)
    {
        errorResponseUnknownTrace(logger, null);
    }

    public static void ErrorResponse(this ILogger logger, string? errorDescription, Exception exception)
    {
        errorResponse(logger, errorDescription, exception);
    }

    public static void FailedToRetrieveChunkError(this ILogger logger, int? chunkIndex, int? chunkTotalCount, HttpStatusCode? httpStatusCode, string? reasonPhrase)
    {
        failedToRetrieveChunkError(logger, chunkIndex, chunkTotalCount, httpStatusCode, reasonPhrase, null);
    }

    public static void TraceCacheHitOrMiss(this ILogger logger, Uri infoLinkUri, bool isHit)
    {
        traceCacheHitOrMiss(logger, infoLinkUri, isHit, null);
    }
}

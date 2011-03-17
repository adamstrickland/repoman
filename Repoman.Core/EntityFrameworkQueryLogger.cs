using System;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using log4net;

namespace Repoman.Core
{
    public class EntityFrameworkQueryLogger
    {
        protected static ILog Log = LogManager.GetLogger("EntityFramework");

        protected static TResult LogQuery<TResult>(IQueryable query, Func<TResult> execute)
        {
            if (Log.IsDebugEnabled)
            {
                var objectQuery = query as ObjectQuery;
                if (objectQuery != null)
                {
                    // Time the execution.
                    int begin = Environment.TickCount;
                    TResult result = execute();
                    int end = Environment.TickCount;
                    int ticks = unchecked(end - begin);	// Allow the calculation to wrap.

                    // Get the caller of this method.
                    StackFrame caller = new StackTrace().GetFrame(3);
                    string sql = objectQuery.ToTraceString();
                    ThreadContext.Properties["hashcode"] = sql.GetHashCode();
                    ThreadContext.Properties["duration"] = ticks;

                    var resultString = string.IsNullOrEmpty(caller.GetFileName())
                                           ? string.Format("{0} {1}.{2} {3}ms\r\n{4}", sql.GetHashCode(),
                                                           caller.GetMethod().DeclaringType != null ? caller.GetMethod().DeclaringType.FullName : string.Empty,
                                                           caller.GetMethod().Name, ticks, sql)
                                           : string.Format("{0} {1}.{2} ({3} {4}) {5}ms\r\n{6}",
                                                           sql.GetHashCode(),
                                                           caller.GetMethod().DeclaringType != null ? caller.GetMethod().DeclaringType.FullName : string.Empty,
                                                           caller.GetMethod().Name,
                                                           caller.GetFileName(),
                                                           caller.GetFileLineNumber(),
                                                           ticks,
                                                           sql);

                    Log.Debug(resultString);

                    return result;
                }
            }

            // If not logging, just execute the query.
            return execute();
        }
    }
}

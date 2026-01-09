/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace LogWatcher
{
    public interface ISearcher
    {
        void Reset(bool enabled);
    }
}

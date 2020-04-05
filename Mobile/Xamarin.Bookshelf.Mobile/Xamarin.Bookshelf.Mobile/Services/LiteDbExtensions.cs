using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Bookshelf.Mobile.Services
{
    public static class LiteDbExtensions
    {
        public static Task<T> FindByIdAsync<T>(this ILiteCollection<T> collection, string id)
        {
            var tsc = new TaskCompletionSource<T>();
            try
            {
                T row = collection.FindById(id);
                tsc.SetResult(row);
            }
            catch (Exception ex)
            {
                tsc.SetException(ex);
            }

            return tsc.Task;
        }

        public static Task<int> UpsertAsync<T>(this ILiteCollection<T> collection, IEnumerable<T> items)
        {
            var tsc = new TaskCompletionSource<int>();
            try
            {
                int count = collection.Upsert(items);
                tsc.SetResult(count);
            }
            catch (Exception ex)
            {
                tsc.SetException(ex);
            }
            return tsc.Task;
        }

        public static Task<IEnumerable<T>> ToListAsync<T>(this ILiteQueryable<T> query)
        {
            var tsc = new TaskCompletionSource<IEnumerable<T>>();
            try
            {
                var list = query.ToList();
                tsc.SetResult(list);
            }
            catch (Exception ex)
            {
                tsc.SetException(ex);
            }
            return tsc.Task;
        }
    }
}
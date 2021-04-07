using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace RapidCore.Mongo
{
    /// <summary>
    /// Mockable wrapper class for an IAsyncCursor
    /// </summary>
    /// <typeparam name="TDocument">The document type associated with the IAsyncCursor</typeparam>
    public class MongoAsyncCursor<TDocument>
    {
        public IAsyncCursor<TDocument> Cursor { get; }

        public MongoAsyncCursor(IAsyncCursor<TDocument> cursor)
        {
            this.Cursor = cursor;
        }

        public virtual bool MoveNext(CancellationToken cancellationToken = new CancellationToken())
        {
            return Cursor.MoveNext(cancellationToken);
        }

        public virtual Task<bool> MoveNextAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Cursor.MoveNextAsync(cancellationToken);
        }

        public virtual IEnumerable<TDocument> Current => Cursor.Current;

        public virtual bool Any(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.Any(cancellationToken);
        }
        
        public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.AnyAsync(cancellationToken);
        }
        
        public virtual TDocument First(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.First(cancellationToken);
        }

        public virtual Task<TDocument> FirstAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.FirstAsync(cancellationToken);
        }

        public virtual TDocument FirstOrDefault(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.FirstOrDefault(cancellationToken);
        }

        public virtual Task<TDocument> FirstOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task ForEachAsync(Func<TDocument, Task> processor, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ForEachAsync(processor, cancellationToken);
        }

        public virtual Task ForEachAsync(Func<TDocument, int, Task> processor, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ForEachAsync(processor, cancellationToken);
        }

        public virtual Task ForEachAsync(Action<TDocument> processor, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ForEachAsync(processor, cancellationToken);
        }

        public virtual Task ForEachAsync(Action<TDocument, int> processor, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ForEachAsync(processor, cancellationToken);
        }

        public virtual TDocument Single(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.Single(cancellationToken);
        }

        public virtual Task<TDocument> SingleAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.SingleAsync(cancellationToken);
        }

        public virtual TDocument SingleOrDefault(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.SingleOrDefault(cancellationToken);
        }

        public virtual Task<TDocument> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.SingleOrDefaultAsync(cancellationToken);
        }

        public virtual IEnumerable<TDocument> ToEnumerable(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ToEnumerable(cancellationToken);
        }

        public virtual List<TDocument> ToList(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ToList(cancellationToken);
        }

        public virtual Task<List<TDocument>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Cursor.ToListAsync(cancellationToken);
        }
    }
}
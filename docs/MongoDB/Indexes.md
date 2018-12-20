# Indexes

Having indexes is crucial to query performance.

With `RapidCore.Mongo` you can define your indexes using attributes on your entity POCOs.


Note that

All key indexes are made in ascending order.
All indexes are made in the background
By default indexes are not sparse

#### Examples

- [The individual index to properties](../Examples#the-individual-index-to-properties)
- [Compound indexes](../Examples#compound-indexes)
- [Indexes on nested documents](../Examples#indexes-on-nested-documents)
- [Ensure the indexes are created](../Examples#ensure-the-indexes-are-created)
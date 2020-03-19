# MimeTypes

When dealing with file uploads, it is often fairly important to **know** what type of file you are actually dealing with. Trusting the file extension is not always good enough.

To deal with this issue we have `RapidCore.IO.MimeTyper`, which tries to figure out the [mime-type](https://en.wikipedia.org/wiki/Media_type) of a blob of bytes or a base64 encoded string without inspecting the entire thing. It does this by looking for specific _signatures_.


## Tell me the mime-type of my bytes, please

To get the actual mime-type of a given sequence of bytes (i.e. a file), you simply call the `GetMimeType` method. It takes the bytes and optionally the filename. It only uses the filename if it finds itself in a situation, where there are multiple candidates based on the signature alone (e.g. with _OpenXML_ which are zip archives without a special signature).

```csharp
var mimeTyper = new RapidCore.IO.MimeTyper();
byte[] myBytes = read_file("actually_a.jpg");
var myFilename = "something.png";

var mimeType = mimeTyper.GetMimeType(myBytes, myFilename);
// => image/jpeg
```


## Tell me the mime-type of my base64 encoded bytes, please

These days, file uploads are often done by base64 encoding the file contents on the client side and then "uploading" a string. In those cases, we might want to check the mime-type _before_ we decode it. The `MimeTyper` can help you with that :)

```csharp
var mimeTyper = new RapidCore.IO.MimeTyper();
string base64EncodedBytes = base64_encode( read_file("actually_a.jpg") );
var myFilename = "something.png";

var mimeType = mimeTyper.GetMimeType(base64EncodedBytes, myFilename);
// => image/jpeg
```


## But, I only have a filename

In the case where you do not actually have the bytes handy and you are ok with a _shallow_ mime-type check, you can use...

```csharp
var mimeTyper = new RapidCore.IO.MimeTyper();

var mimeType = mimeTyper.GetMimeTypeFromFilename("something.png");
// => image/png
```

## When you have a number of mime-types that are OK

If you have some sort of upload situation, where you have a whitelist of allowed mime-types, you can use...

```csharp
var mimeTyper = new RapidCore.IO.MimeTyper();
string base64EncodedBytes = base64_encode( read_file("actually_a.gif") );
var myFilename = "something.png";

if (!mimeTyper.IsMimeTypeOneOfTheseFromBase64(base64EncodedBytes, myFilename, "image/jpg", "image/png"))
{
    throw new IamNotAngryIamDisappointedException();
}

//
// alternative syntax
//

var allowedTypes = new [] { "image/jpeg", "image/png" };

if (!mimeTyper.IsMimeTypeOneOfTheseFromBase64(base64EncodedBytes, myFilename, allowedTypes))
{
    throw new IamNotAngryIamDisappointedException();
}
```

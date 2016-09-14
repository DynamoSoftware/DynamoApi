# Dynamo API (v1)

This is a pubic API to Dynamo Software's Dynamo (http://www.dynamosoftware.com/).

In order to use the API, you need a Dynamo account which has granted API access. Please, your administrator or Dynamo Software support for instructions how to acquire one.

This project features code samples in several technologies:
   * JavaScript - for web pages and server-side scripting.
   * C# (.Net Framework) - for desktop tools, backend processing etc.  

# Change log

   * [pending] Official release is expected with the Dynamo 7.0 release.
   * Initial version

# Description

This is a high-level overview of the provided API. For detail description of the format of the input and output arguments, please check the specific sample code provided with this project. 

Login
-----
Performs a login into existing tenant. If the login is successful, will return a session token. This session token will also be assigned as a HTTP-only cookie.

   * Method: POST
   * URL: /v1/login
   * Body: userName=<code>username</code>&password=<code>password</code>&tenant=<code>tenant</code>
   * Response: application/json
   
   <code>{ sidt: 'sessionToken' }</code>

Save
-----
Creates or updates an item. The item is identified by <code>es</code> and <code>id</code>. The properties to be set are provided as a key/value pair.

   * Method: POST
   * URL: /v1/save
   * Body: Stringified JSON object that must contain <code>es</code> and <code>id</code> as a minimum.
   * Response: application/json
   
   <code>{ dynamoId: 'the id of the created/updated item', es: 'The type of the item' }</code>

GetById
----------------
Returns an item with the specified entityName and dynamoId.

   * Method: GET
   * URL: /v1/getbyid
   * Query string: entityName=<code>entityName</code>&dynamoId=<code>dynamoId</code>
   * Header: <code>x-columns</code> a comma-separated list of properties to return.
   * Response: application/json                      

   <code>{ id: 'the id of the requesed item', es: 'The type of the requesed item' }</code> and the rest of the requested property/value pairs. 
   
GetByTemplate
----------------
Returns item(s) that match specific template. The items are identified by one or more property/value pairs. 

   * Method: GET
   * URL: /v1/getbytemplate
   * Query string: Stringified JSON object that must contain one or more pairs of property/value.
   * Header: <code>x-columns</code> a comma-separated list of properties to return. 
   * Response: application/json
   
   <code>{ totalCount: 'number of items', items: 'an array of items' }</code> 

GetDocument
--------
Returns a file for the specified document id. 

   * Method: GET
   * URL: /v1/GetDocument?id=<code>document id</code>
   * Response: the file

GetDocuments
--------
Returns a zip file for the specified document ids. 

   * Method: GET
   * URL: /v1/GetDocuments?docIds=<code>document ids</code>
   * Response: the zip file

Execute Command
--------
Executes a command/workflow logic defined in Dynamo. 

   * Method: POST
   * URL: /v1/executecommand
   * Body: Stringified JSON object that must contain <code>commandName</code> and one or more pairs of property/value.
   * Response: application/json
   
   <code>{ err: 'true/false', errMessage: 'Exception message if any' }</code>
   

# MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

No conditions.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



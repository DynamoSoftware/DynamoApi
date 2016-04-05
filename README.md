# Dynamo Api v1

Public API to access Dynamo.

# Description

This example project demonstrates the usage of the Dynamo API via several technologies: node, C#.

Login
-----
Performs a login into existing tenant. If successful, a session cookie will be assigned to the response.

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
   
   <code>{ dynamoId: 'the id of the created/updated item' }</code>

GetByTemplate
----------------
Returns item(s) that match specific template. The item is identified by one or more property/value pairs. 

   * Method: POST
   * URL: /v1/getbytemplate
   * Body: Stringified JSON object that must contain one or more pair of property/value.
   * Header: <code>x-columns</code> a semicolon separated list of properties. 
   * Response: application/json
   
   <code>{ es: 'The type of the item', id: 'the id of the item' }</code> and the test of the property/value pairs requested. 

Download
--------
Returns item(s) that match specific template. The item is identified by one or more property/value pairs. 

   * Method: GET
   * URL: /v1/document/download?dynamoId=<code>document id</code>
   * Response: application/gzip //TODO
   

# License




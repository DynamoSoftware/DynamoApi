Dynamo API (v2) For Developers
==============================

This is a high-level overview of the provided pubic API to Dynamo Software’s Dynamo application.

In order to use the API, you need a [Dynamo API Key](http://docs.netagesolutions.com/generate-a-dynamo-api-key/). Please, contact your Dynamo Software representative to acquire one.

[](https://github.com/DynamoSoftware/DynamoAPI#how-to-use-it)Endpoint URIs
--------------------------------------------------------------------------

All URI’s are relative to **https://api.dynamosoftware.com/api/v2.0**

Example: **https://api.dynamosoftware.com/api/v2.0/entity**

[](https://github.com/DynamoSoftware/DynamoAPI#api-reference)API Reference
--------------------------------------------------------------------------

### [](https://github.com/DynamoSoftware/DynamoAPI#login)Authorization

Each request needs to be sent with authorization header, which grants user access to Dynamo.

Header:  Authorization  – Bearer 'API KEY'

[](https://github.com/DynamoSoftware/DynamoAPI#save)API Swagger
---------------------------------------------------------------

You can see all production-ready Endpoints in the API Swagger:

[https://api.dynamosoftware.com/swagger](https://api.dynamosoftware.com/swagger)

The swagger also contains a brief explanation of the request methods, along with their parameters and response models.

[](https://github.com/DynamoSoftware/DynamoAPI#delete)Usage Examples
--------------------------------------------------------------------

### Document Upload

Uploads a document to Dynamo and returns the newly created document.

*   Method: POST
*   URL: /entity/Document
*   Body: Stringified JSON object. Mind that the following properties are required: “_content”, “Extension” and “Title”.
*   Response: application/json
```json
{
    "_content": "the document in Base64 encoded format",
    "Title": "name of the document",
    "Extension": "extension of the document"
}
```
### Update fields of type Multi-Select Dropdown/Search field, Multi-Select

Updates the values in a multi-select field and returns the updated property fields.

*   Method: PUT
*   URL: /entity/{entityName}/{id}
*   Body: Stringified JSON object.
*   Response: application/json
```json
{
    "the multi-select property name": [
        {
            "id": "id of entity 1",
            "es": "entity schema name"
        },
        {
            "id": "id entity 2",
            "es": "entity schema name"
        }
    ]
}
```
### Update fields of type Single-Select Dropdown

Updates the value in a single-select field and returns the updated property fields.

*   Method: PUT
*   URL: /entity/{entityName}/{id}
*   Body: Stringified JSON object.
*   Response: application/json
```json
{
    "the single-select property name": {
        "id": "id of the lookup value",
        "es": "lookup entity schema name"
    }
}
```
### Advanced search via API Query

Returns the entites that match a given advanced search criteria. In the current example the search will return only Activities for which the property “Name (ID)” contains “Activity”.

*   Method: POST
*   URL: /search
*   Body: Stringified JSON object that represents an advanced search criteria.
*   Header: x-columnsa comma-separated list of properties to return.
*   Response: application/json
```json
{
    "advf": {
        "e": [
            {
                "_name": "Activity",
                "rule": [
                    {
                        "_op": "all",
                        "_prop": "Name (ID)",
                        "values": [
                            "Activity"
                        ]
                    }
                ]
            }
        ]
    }
}
```
### List All Entities

Lists all available entities in the tenant.

*   Method: GET
*   URL: /entity
*   Response: application/json

### List All Fields

Lists all fields for the Contact entity.

*   Method: GET
*   URL: /entity/Contact/Properties
*   Response: application/json

### List All Entities of Type

Lists all contacts in the tenant.

*   Method: GET
*   URL: /entity/Contact?all=true
*   Response: application/json

### List All Entities of Type (specific properties)

Lists all contacts in the tenant but only returns the FirstName and LastName properties.

*   Method: GET
*   URL: /entity/Contact?all=true
*   Headers:
    *   x-colums: FirstName, LastName
*   Response: application/json

### Create Entity

Creates an entity of type Contact and returns the created instance.

*   Method: POST
*   URL: /entity/Contact
*   Headers:
    *   Content-Type: application/json
*   Body:
```json
{
   "FirstName": "John",
   "LastName": "Doe"
} 
```
*   Response: application/json

### Update Entity

Updates a Contact entity with Internal ID “00000000-0000-0000-0000-000000000000” and returns the updated instance.

*   Method: PUT
*   URL: /entity/Contact/00000000-0000-0000-0000-000000000000
*   Headers:
    *   Content-Type: application/json
*   Body:
```json
{
   "FirstName": "Rick",
   "LastName": "Doe"
}
```
*   Response: application/json

### Delete Entity

Deletes a Contact entity with Internal ID “00000000-0000-0000-0000-000000000000”.

*   Method: DELETE
*   URL: /entity/Contact/00000000-0000-0000-0000-000000000000
*   Response: application/json

### Relate Two Entities

Relates a contact with Internal ID “00000000-0000-0000-0000-000000000000” to a company with Internal ID ”00000000-0000-0000-0000-000000000001”.

*   Method: POST
*   URL: /entity/Investor_Contact
*   Headers:
    *   Content-Type: application/json
*   Body:
```json
{
   "_id1": "00000000-0000-0000-0000-000000000001",
   "_id2": "00000000-0000-0000-0000-000000000000"
}
```
*   Response: application/json

### Relate Two Entities with Roles

Relates a contact with Internal ID “00000000-0000-0000-0000-000000000000” to a company with Internal ID ”00000000-0000-0000-0000-000000000001”. Assigns two roles to the relation with Internal ID's “00000000-0000-0000-0000-000000000002” and “00000000-0000-0000-0000-000000000003”.

*   Method: POST
*   URL: /entity/Investor_Contact
*   Headers:
    *   Content-Type: application/json
*   Body:
```json
{
    "_id1": "00000000-0000-0000-0000-000000000001",
    "_id2": "00000000-0000-0000-0000-000000000000",
    "Roles": [
        {
            "es": "Roles",
            "id": "00000000-0000-0000-0000-000000000002"
        },
        {
            "es": "Roles",
            "id": "00000000-0000-0000-0000-000000000003"
        }
    ]
}
```
*   Response: application/json

### Delete a Relation

Deletes a relation with Relation ID “00000000-0000-0000-0000-000000000000,00000000-0000-0000-0000-000000000001,2”.

*   Method: DELETE
*   URL: /entity/Investor_Contact/00000000-0000-0000-0000-000000000000,00000000-0000-0000-0000-000000000001,2
*   Headers:
    *   Content-Type: application/json
*   Response: application/json

### Create Triple Relation

Relates a contact with Internal ID “00000000-0000-0000-0000-000000000000” to a company with Internal ID ”00000000-0000-0000-0000-000000000001” to a Role with Internal ID “00000000-0000-0000-0000-000000000002”.

*   Method: POST
*   URL: /entity/Investor_Contact_Roles
*   Headers:
    *   Content-Type: application/json
*   Body:
```json
{
   "_id1": "00000000-0000-0000-0000-000000000001",
   "_id2": "00000000-0000-0000-0000-000000000000",
   "_id3": "00000000-0000-0000-0000-000000000002"
}
```
*   Response: application/json

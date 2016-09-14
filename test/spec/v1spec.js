const tenant = require("./helpers/testsubject.js");
const request = require('request');
const querystring = require('querystring');
const https = require('https');
const fs = require('fs');
const contentDisposition = require('content-disposition');

var sessionCookies = null;

var apiGet = function (name, args, headers, cb) {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 30000;

    if (!cb) {
        cb = headers;
        headers = null;
    }

    if (name[0] != '/') name = '/' + name;

    var query = '';
    if (args) {
        query = '?' + querystring.stringify(args);
    }

    var options = { url: tenant.apiurl + name + query };
    if (sessionCookies) {
        options.headers = { 'Cookie': sessionCookies };
    }

    if (headers) {
        for (var key in headers) {
            if (headers.hasOwnProperty(key)) {
                options.headers = options.headers || {};
                options.headers[key] = headers[key];
            }
        }
    }

    request.get(options, function (err, res, body) {

        //console.log('err: ' + err);
        //console.log('body: ' + body);
        //console.log('statusCode: ' + res.statusCode);
        //console.log('response cookies: ' + JSON.stringify(res.headers['set-cookie']));

        if (err) throw err;

        if (res.statusCode != 200) {
            cb(body);
        } else {

            sessionCookies = res.headers['set-cookie'] || sessionCookies;

            if (body) body = JSON.parse(body);
            cb(null, body);
        }

    });
};

var apiPostJson = function (name, args, cb) {

    if (name[0] != '/') name = '/' + name;

    var options = { url: tenant.apiurl + name, json : args };
    if (sessionCookies) {
        options.headers = { 'Cookie': sessionCookies };
    }

    request.post(options, function (err, res, body) {

        //console.log('err: ' + err);
        //console.log('body: ' + body);
        //console.log('statusCode: ' + res.statusCode);
        //console.log('response cookies: ' + JSON.stringify(res.headers['set-cookie']));

        if (err) throw err;

        if (res.statusCode != 200) {
            cb(body);
        } else {

            sessionCookies = res.headers['set-cookie'] || sessionCookies;

          //  if (body) body = JSON.parse(body);
            cb(null, body);
        }

    });

};

describe("login - ", function () {

    it('with invalid credentials', function (done) {

        var args = {
            username: 'these is no such user there',
            password: tenant.password,
            tenant: tenant.name
        };

        //POST login
        //  In case of an error, the statusCode will be 500 and the body could contain a human readable description

        apiPostJson('login', args, function (err, data) {
            expect(err).toBeTruthy();
            done();
        });

    });

    it('with valid credentials', function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        //POST login
        //  upon success returns an object with a session token

        apiPostJson('login', args, function (err, data) {
            expect(err).toBeFalsy();
            expect(data.sidt).toBeTruthy();
            done();
        });


    });


});

describe("read - ", function () {

    it("getbyid should return existing item", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {
            es: 'Contact',
            id: 'D7B0576D-1910-436A-8E64-A90619960F05',
            fullname: 'Indiana Jones'
        };

        apiPostJson('login', args, function (err) {

            apiPostJson('save', item, function (err) {

                apiGet('getbyid', { entityName: item.es, dynamoId: item.id }, { 'x-columns': 'fullname' }, function (err, data) {

                    expect(err).toBeFalsy();

                    expect(data.fullname).toBe(item.fullname);

                    done();
                });

            });

        });

    });

    it("GetByTemplate should return all items of type Contact", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {
            es: 'Contact',
            id: 'D7B0576D-1910-436A-8E64-A90619960F05',
            fullname: 'Indiana Jones'
        };

        apiPostJson('login', args, function (err) {

            apiPostJson('save', item, function (err) {

                apiGet('getByTemplate', { es: 'Contact' },
                    { 'x-columns': 'ID,First Name,Last Name' }, function (err, getData) {

                        expect(err).toBeFalsy();
                        expect(getData.totalCount).toBe(2);

                        expect(getData.items[0].ID.toLowerCase()).toBe(item.id.toLowerCase());
                        expect(getData.items[0]['First Name']).toBe('Indiana');
                        expect(getData.items[0]['Last Name']).toBe('Jones');
                        done();
                    });

            });

        });

    });

    it("GetByTemplate should return all items of type Contact with firstname 'Indiana'", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        apiPostJson('login', args, function (err) {

            apiGet('getByTemplate', {
                es: 'contact',
                firstname: 'Indiana'
                // id: ['D7B0576D-1910-436A-8E64-A90619960F05']
            }, { 'x-columns': 'ID,First Name,Last Name' }, function (err, getData) {

                expect(err).toBeFalsy();
                expect(getData.totalCount).toBe(1);

                expect(getData.items[0].ID.toLowerCase()).toBe('D7B0576D-1910-436A-8E64-A90619960F05'.toLowerCase());
                expect(getData.items[0]['First Name']).toBe('Indiana');
                expect(getData.items[0]['Last Name']).toBe('Jones');
                done();
            });
        });

    });

});

describe("write - ", function () {

    it("save should create items", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {
            es: 'Contact',
            id: '48a052cb-b3c2-47de-9e05-bb518fe11160',
            fullname: 'john smith'
        };

        apiPostJson('login', args, function (err, data) {

            apiPostJson('save', item, function (err, savedData) {

                expect(err).toBeFalsy();
                expect(savedData[0].dynamoId).toBe(item.id);

                done();

            });

        });

    });

    it("save should update existing items", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {
            es: 'Contact',
            id: '48a052cb-b3c2-47de-9e05-bb518fe11160',
            fullname: 'john smith1'
        };

        apiPostJson('login', args, function (err, data) {

            apiPostJson('save', item, function (err, savedData) {

                expect(err).toBeFalsy();
                expect(savedData[0].dynamoId).toBe(item.id);

                apiGet('getbyid', { entityName: item.es, dynamoId: item.id }, { 'x-columns': 'fullname' }, function (err, getData) {

                    expect(err).toBeFalsy();

                    expect(getData.fullname).toBe(item.fullname);

                    done();
                });

            });
        });
    });

    it("save should accept array of items", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var items = [
            {
                es: 'Contact',
                id: '48a052cb-b3c2-47de-9e05-bb518fe11160',
                fullname: 'john smith2'
            },
            {
                es: 'Activity',
                id: 'dc6c77a3-2173-478b-8084-b54174e064a2',
                subject: 'Test Subject',
                body: 'Test Body',
                to: 'test@ivoe.com',
            }
        ];

        apiPostJson('login', args, function (err, data) {

            apiPostJson('save', items, function (err, savedData) {

                expect(err).toBeFalsy();

                var testItem = items[0];
                apiGet('getbyid', { entityName: savedData[0].es, dynamoId: savedData[0].dynamoId }, { 'x-columns': 'fullname' }, function (err, getData) {

                    expect(err).toBeFalsy();
                    expect(getData.fullname).toBe(testItem.fullname);
                    testItem = items[1];
                    apiGet('getbyid', { entityName: savedData[1].es, dynamoId: savedData[1].dynamoId }, { 'x-columns': 'to,body,subject' }, function (err, getData1) {

                        expect(err).toBeFalsy();

                        expect(getData1.to).toBe(testItem.to);
                        expect(getData1.subject).toBe(testItem.subject);
                        expect(getData1.body).toBe(testItem.body);

                        done();
                    });

                });

            });
        });
    });
});

describe("download - ", function () {

    it("should get existing file by id", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {
            es: 'Document',
            id: '1dcded01-9b17-48e3-9180-495e4ee54f39'
        };
        var dest_dir = './downloads/';

        apiPostJson('login', args, function (err) {

            if (!fs.existsSync(dest_dir)) {
                fs.mkdirSync(dest_dir);
            }
            var options = { url: tenant.apiurl + '/GetDocument?id=' + item.id };
            if (sessionCookies) {
                options.headers = { 'Cookie': sessionCookies };
            }

            request.get(options).on('response', function (response) {

                var contentDispositionHeader = contentDisposition.parse(response.headers['content-disposition'].toString());
                var file_name = contentDispositionHeader.parameters['filename'];

                var dest = dest_dir + file_name;

                var writeStream = fs.createWriteStream(dest);

                writeStream.on('finish', function () {
                    fs.stat(dest, function (fserr, stats) {
                        expect(fserr).toBeFalsy();
                        expect(stats.isFile()).toBeTruthy();

                        expect(stats.size > 0).toBeTruthy();
                        done();
                    });                    
                });


                writeStream.on('error', function (err) {
                    fs.unlink(dest);
                });

                response.pipe(writeStream);
            });

        });

    });
     it("should get existing files by ids in a Zip", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var item = {          
            ids: '1dcded01-9b17-48e3-9180-495e4ee54f39;82c88b76-83ea-44c3-9a44-a5c7217b253e;720ddf69-ae44-4b2f-9c2c-ed295ff2c8d8'
        };
        var dest_dir = './downloads/';

        apiPostJson('login', args, function (err) {

            if (!fs.existsSync(dest_dir)) {
                fs.mkdirSync(dest_dir);
            }
            var options = { url: tenant.apiurl + '/GetDocuments?docIds=' + item.ids };
            if (sessionCookies) {
                options.headers = { 'Cookie': sessionCookies };
            }

            request.get(options).on('response', function (response) {               

                var contentDispositionHeader = contentDisposition.parse(response.headers['content-disposition'].toString());
                var file_name = contentDispositionHeader.parameters['filename'];
               
                var dest = dest_dir + file_name;

                var writeStream = fs.createWriteStream(dest);

                writeStream.on('finish', function () {
                    fs.stat(dest, function (fserr, stats) {
                        expect(fserr).toBeFalsy();
                        expect(stats.isFile()).toBeTruthy();

                        expect(stats.size > 0).toBeTruthy();
                        done();
                    });                    
                });


                writeStream.on('error', function (err) {
                    fs.unlink(dest);
                });

                response.pipe(writeStream);
            });

        });

    });

});

describe("Execute - ", function () {
    it("should execute workflow", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };


        var workflow = {
            es: 'Workflow Logic',
            id: 'dd115c36-4cbd-4623-b028-936c16455b06',
            Name: 'ExistingWorkflow',
            Enabled: true,
            WorkflowDefinition: "\
trigger \n\
   operation <- 'APIOperation'"
        };

        apiPostJson('login', args, function (err, data) {
            //Create workflow to call later with ExecuteCommand
            apiPostJson('save', workflow, function (saveErr, savedData) {
                expect(saveErr).toBeFalsy();
                expect(savedData[0].dynamoId).toBe(workflow.id);
                
                //Define the command with parameters
                var command = {
                    commandName: workflow.Name,
                    id: '48a052cb-b3c2-47de-9e05-bb518fe11160',
                    fullname: 'john smith',
                    param1: 'par_1',
                };
                apiPostJson('ExecuteCommand', command, function (execErr, respData) {

                    expect(execErr).toBeFalsy();
                    expect(respData.err).toBeFalsy();
                  
                    done();
                });
            });
        });
    });

    it("should return an error for nonexisting workflow", function (done) {

        var args = {
            username: tenant.username,
            password: tenant.password,
            tenant: tenant.name
        };

        var command = {
            commandName: 'NonExisingWorkflow',
            id: '48a052cb-b3c2-47de-9e05-bb518fe11160',
            fullname: 'john smith',
            param1: 'par_1',
        };

        apiPostJson('login', args, function (err, data) {
            apiPostJson('ExecuteCommand', command, function (execError, respData) {

                expect(execError).toBeFalsy();
                expect(respData.err).toBeTruthy();
                expect(respData.errMessage).toMatch('NonExisingWorkflow');
               
                done();
            });
        });
    });
});


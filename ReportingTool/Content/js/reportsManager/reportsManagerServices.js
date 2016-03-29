'use strict';

reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields,
        getJiraUsers: getJiraUsers,
        // -------------------------------------------------------------------------------------
        getTemplateFields: getTemplateFields,   //  debug
        getUserActivityRequest: getUserActivityRequest,       //  debug
        //TestActivityRequest: TestActivityRequest,     //  debug
        getIssuesRequest: getIssuesRequest     //  debug
    };

    function getTeams() {
        return $http.get("Teams/GetAllTeams");
    }

    function getTemplates() {
        return $http.get("Templates/GetAllTemplates");
    }

    function getFields(templateId) {
        //console.log ($http.get("Templates/GetTemplateFields?templateId=" + templateId) );   //  debug
        return $http.get("Templates/GetTemplateFields?templateId=" + templateId);
    }

    function getJiraUsers(param) {
        return $http.get("JiraUsers/GetAllUsers", { params: { "searchValue": param } });
    }

    //  ---------------------------------------------------------------------------------------------------------------  //  debug
    //  2
    function getTemplateFields(templateId) {
        var fields = $http.get("Templates/GetTemplateFields?templateId=" + templateId);
        return fields;
    }
    
    function getUserActivityRequest(userName, dateFrom, dateTo) {
        //  --- working example of opastukhov ------------------------------------
        //$scope.Test = function () {
        //    var req = {
        //        url: '/Reports/GetUserActivity?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
        //        method: 'GET',
        //        headers: { 'content-type': 'application/json' }
        //    };

        //    $http(req).then(
        //        function (r) {
        //            alert("ok");
        //        },
        //        function (response) {
        //            alert("not ok");
        //        }
        //     );
        //};
        //  --- working example of opastukhov ------------------------------------
            
            //  WORKS OK!
            //var req = {
            //    url: '/Reports/GetUserActivity?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
            //    method: 'GET',
            //    headers: { 'content-type': 'application/json' }
            //};

        var urlVar = '/Reports/GetUserActivity?userName=' + userName + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo;
            var req = {
                //url: '/Reports/GetUserActivity?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
                url: urlVar,
                method: 'GET',
                headers: { 'content-type': 'application/json' }
            };

            //$http(req).then(
            //    function (r) {
            //        //alert("ok" + " r = " + r);
            //        console.log(r);
            //        userActivity = response.data.Timespent;
            //    },
            //    function (response) {
            //        console.log("Error getUserActivity.");
            //    }
            // );
             // -------------------------------------------------------------------------

          //return $http(req);

        // OLD
        // return $http.get("Reports/GetUserActivity?userName=" + userName);

        //   + "&dateFrom= " + "2016-01-01" + "&dateTo=" + dateTo + "2016-03-01"
        //  + "&dateFrom='" + dateFrom + "'&dateTo='" + dateTo + "'"
        
            var response = $http(req);
            return response;
    }

    function getIssuesRequest(userName, dateFrom, dateTo) {
        //  --- working example of opastukhov ------------------------------------
        //$scope.Test = function () {
        //    var req = {
        //        url: '/Reports/GetIssues?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
        //        method: 'GET',
        //        headers: { 'content-type': 'application/json' }
        //    };

        //    $http(req).then(
        //        function (r) {
        //            alert("ok");
        //        },
        //        function (response) {
        //            alert("not ok");

        //        }
        //     );
        //};
        //  --- working example of opastukhov ------------------------------------

        var urlVar = '/Reports/GetIssues?userName=' + userName + '&dateFrom=' + dateFrom + '&dateTo=' + dateTo;
        var req = {
            //url: '/Reports/GetIssues?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
            url: urlVar,
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        };

        var response = $http(req);
        return response;
    }

    //function TestActivityRequest() {
    //    var req = {
    //        url: '/Reports/GetUserActivity?userName=amarutc&dateFrom=2016-03-21&dateTo=2016-03-24',
    //        method: 'GET',
    //        headers: { 'content-type': 'application/json' }
    //    };

    //    $http(req).then(
    //        function (r) {
    //            var tmpData = r;
    //            console.log(r.data.Timespent);

    //            console.log("TestActivityRequest success");
    //        },
    //        function (response) {
    //            console.log("TestActivityRequest fail");
    //        }
    //     );
    //};

    return reportsFactory;
}]);


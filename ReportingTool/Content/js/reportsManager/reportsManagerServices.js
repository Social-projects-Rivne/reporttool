'use strict';

reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields,
        getUserActivityRequest: getUserActivityRequest,       //  debug
        getIssuesRequest: getIssuesRequest,     //  debug
        getTemplateFields: getTemplateFields   //  debug
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

    //  --- a method from TemplateFactory -----------------------------------------------------------   //  debug
    //  1
    //function getTemplateFields(templateId) {
    //    var promise1 = $http.get("Templates/GetTemplateFields?templateId=" + templateId);   //  debug
    //    console.log(promise1);   //  debug

    //    return $http.get("Templates/GetTemplateFields?templateId=" + templateId)
    //    .then(function (response) {
    //        if (typeof response.data === 'object') {
    //            return response.data;
    //        } else {
    //            return $q.reject(response.data);
    //        }
    //    });
    //}

    //  2
    function getTemplateFields(templateId) {
        var fields = $http.get("Templates/GetTemplateFields?templateId=" + templateId);
        return fields;
    }
    //  ---------------------------------------------------------------------------------------------------------------  //  debug

    //  BE params
    // GetUserActivity(string userName, string dateFrom, string dateTo)

    //  example
    //function getTeams() {
    //    return $http.get("Teams/GetAllTeams");
    //}

    // v.001
    //function getUserActivity(userName, dateFrom, dateTo) {
    //    return $http.get("Reports/GetUserActivity?userName=" + userName + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo)
    //    .then(function (response) {
    //        if (typeof response.data === 'object') {
    //            return response.data;
    //        } else {
    //            return $q.reject(response.data);
    //        }
    //    });
    //}

    
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

            var req = {
                url: '/Reports/GetUserActivity?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
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

        var req = {
            url: '/Reports/GetIssues?userName=opastutc&dateFrom=2016-01-01&dateTo=2016-03-22',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        };

        var response = $http(req);
        return response;
    }

    return reportsFactory;
}]);


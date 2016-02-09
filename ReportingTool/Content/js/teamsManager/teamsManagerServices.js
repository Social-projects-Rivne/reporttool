'use strict';

teamsManagerModule.
factory('TeamFactory', ['$http', function ($http) {
    var teamFactory = {
        GetAllTeams: GetAllTeams,
        editTeam: editTeam,
        deleteTeam: deleteTeam,
        createTeam: createTeam
    };

    function GetAllTeams() {
        var teams = $http.get("Teams/GetAllTeams");
        return teams;
    }

    function createTeam(newTeam) {
        return $http({
            url: 'Teams/AddNewTeam',
            method: 'POST',
            data: newTeam,
            headers: { 'content-type': 'application/json' }
        });
    }

    function editTeam(data) {
        return $http.put("Teams/EditTeam", {
            data: data
        });
    }

    function deleteTeam(id) {
        return $http.put("Teams/DeleteTeam/" + id);
    }

    return teamFactory;
}]);

teamsManagerModule.factory('TemplatesFactory', ['$http', function ($http) {
    var template = {
        all: all,
        create: create,
        update: update,
        del: del
    };

    function all() {
        return $http.get("http://localhost:3000/templates/");
    }

    function create(data) {
        return $http.post("http://localhost:3000/templates/", data = data);
    }

    function update(data) {
        return $http.put("http://localhost:3000/templates/", data = data);
    }

    function del(id) {
        return $http.delete("http://localhost:3000/templates/" + id);
    }

    return template;
}]);

teamsManagerModule.service('JiraUsersService', function () {
    var jiraUsers = [{
        userName: 'Loading...',
        fullName: 'Loading...'
    }];
        
    jiraUsers = $http.get("JiraUsers/GetAllUsers").then(Success, Fail);
       
    function Success(response) {
        jiraUsers = response.data;
    }

    function Fail(response) {
        console.error("getUsers error!");
    }

    var getJiraUsers = function () {
        return jiraUsers;
    };

    return {
        getJiraUsers: getJiraUsers
    };
});
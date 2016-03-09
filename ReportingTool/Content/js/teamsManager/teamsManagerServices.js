'use strict';

teamsManagerModule.
factory('TeamFactory', ['$http', function ($http) {
    var teamFactory = {
        GetAllTeams: GetAllTeams,
        updateTeam: updateTeam,
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

    function updateTeam(data) {
        return $http.put("Teams/EditTeam", data);
    }

    function deleteTeam(id) {
        return $http.delete("Teams/DeleteTeam", { params: { id: id } });
    }

    return teamFactory;
}]);

teamsManagerModule.factory('UserFactory', ['$http', function ($http) {
    var jiraUsers = {
        getJiraUsers: getJiraUsers
    };

    function getJiraUsers(param) {
        return $http.get("JiraUsers/GetAllUsers", { params: { "searchValue": param } });
    }

    return jiraUsers;
}]);
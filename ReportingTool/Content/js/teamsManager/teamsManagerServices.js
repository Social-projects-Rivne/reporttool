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
        return $http.delete("Teams/Delete", { params: { id: id } });
    }

    return teamFactory;
}]);

teamsManagerModule.factory('UserFactory', ['$http', function ($http) {
    var jiraUsers = {
        getJiraUsers: getJiraUsers
    };

    function getJiraUsers() {
        return $http.get("JiraUsers/GetAllUsers");
    }

    return jiraUsers;
}]);

teamsManagerModule.factory('TempTeamFactory', function () {

    var tempTeam = {};

    var tempTeamProp = {
            setTempTeam: set,
            getTempTeam: get
    };

    function set(selectedTeam) {
        tempTeam = selectedTeam;
    }

    function get() {
        return tempTeam;
    }

    return tempTeamProp;
});
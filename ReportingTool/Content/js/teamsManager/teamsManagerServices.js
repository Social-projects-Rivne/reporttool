'use strict';

teamsManagerModule.
    factory('TeamFactory', ['$http',
    function ($http) {
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
            return $http.put("Teams/EditTeam", data
            );
        }

        function deleteTeam(id) {
            return $http.delete("Teams/DeleteTeam", { params: { id: id } });
        }

        return teamFactory;
    }]);

teamsManagerModule.
    factory('UserFactory', ['$http',
    function ($http) {
        var jiraUsers = {
            getJiraUsers: getJiraUsers
        };

        function getJiraUsers() {
            return $http.get("JiraUsers/GetAllUsers");
        }

        return jiraUsers;
    }]);

//teamsManagerModule.factory('TemplatesFactory', ['$http', function ($http) {
//    var template = {
//        all: all,
//        create: create,
//        update: update,
//        del: del
//    };

//    function all() {
//        return $http.get("http://localhost:3000/templates/");
//    }

//    function create(data) {
//        return $http.post("http://localhost:3000/templates/", data = data);
//    }

//    function update(data) {
//        return $http.put("http://localhost:3000/templates/", data = data);
//    }

//    function del(id) {
//        return $http.delete("http://localhost:3000/templates/" + id);
//    }

//    return template;
//}]);

teamsManagerModule.factory('TempTeamFactory',
    function () {

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
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

		function createTeam(data) {
		    return $http.post("Teams/AddNewTeam", {
				data: data
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
    }])
	.factory('UserFactory', ['$http', function ($http) {
		var users = {
			all: all
		};

		function all() {
		    var users = $http.get("JiraUsers/GetAllUsers");
		    return users;
		}

		return users;
    }])
	.factory('TemplatesFactory', ['$http', function ($http) {
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
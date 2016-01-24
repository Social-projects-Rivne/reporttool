'use strict';

teamsManagerModule.
factory('TeamFactory', ['$http', function ($http) {
		var teamFactory = {
			all: all,
			get: get,
			update: update,
			del: del,
			create: create
		};

		function all() {
		    debugger;
		    return $http.get("Home/GetAllUsers");
		}

		function get(id) {
			return $http.get("http://localhost:3000/teams/");
			//return $http.get("http://localhost:3000/teams/" + id);
		}

		function create(id, data) {
			return $http.post("/api/teams/", {
				data: data
			});
		}

		function update(id, data) {
			return $http.put("/api/teams/", {
				data: data
			});
		}

		function del(id) {
			return $http.put("/api/teams/" + id);
		}

		return teamFactory;
    }])
	.factory('UserFactory', ['$http', function ($http) {
		var user = {
			all: all
		};

		function all() {
			return $http.get("http://localhost:3000/users/");
		}

		return user;
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
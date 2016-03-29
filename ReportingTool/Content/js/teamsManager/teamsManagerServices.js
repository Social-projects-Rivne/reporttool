'use strict';

teamsManagerModule.
factory('TeamFactory', ['$http', '$uibModal', function ($http, $uibModal) {
    var teamFactory = {
        GetAllTeams: GetAllTeams,
        updateTeam: updateTeam,
        deleteTeam: deleteTeam,
        createTeam: createTeam,
        serverResponse: serverResponse,
        isValid: isValid
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

    var openModal = function (message) {
        $uibModal.open({
            animation: true,
            templateUrl: 'Modal.html',
            controller: 'ModalController',
            resolve: {
                message: function () {
                    return message;
                }
            }
        });
    }

    function isValid(team) {
        var valid = true;
        if (team.teamName == null || team.teamName == '' || team.teamName == ' ') {
            openModal('Team name is incorrect');
            valid = false;
        }
        else if (team.members == null || team.members == '' || team.members == ' ') {
            openModal('No members in team');
            valid = false;
        }
        else {
            for (var i in team.members) {
                if (team.members[i].userName == null || team.members[i].fullName == null || team.members[i].userName == '' || team.members[i].fullName == '' || team.members[i].userName == ' ' || team.members[i].fullName == ' ' || team.members[i].userName == 'Jira users are not loaded' || team.members[i].fullName == 'Jira users are not loaded') {
                    valid = false;
                }
            }
            if (!valid) {
                openModal('Some member is incorrect');
            }
        }
        return valid;
    }

    function serverResponse(response) {
        if (response.data.Answer == 'WrongTeam') {
            openModal('Team is empty');
        }
        if (response.data.Answer == 'WrongName') {
            openModal('Team name is incorrect');
        }
        if (response.data.Answer == 'MembersIsEmpty') {
            openModal('No members in team');
        }
        if (response.data.Answer == 'MembersIsNull') {
            openModal('Some of member is empty');
        }
        if (response.data.Answer == 'MembersIsNotCorrect') {
            openModal('Username or fullname of member is not correct');
        }
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
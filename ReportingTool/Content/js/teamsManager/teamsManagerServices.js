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

    function isValid(team) {
        var valid = null;
        if (team.teamName == null || team.teamName == '' || team.teamName == ' ') {
            return 'Team name is incorrect';
            //valid = false;
        }


        else if (team.members == null || team.members == '' || team.members == ' ') {
            window.alert('No members ');
            //valid = false;
        }
        else {
            for (var i in team.members) {
                if (team.members[i].userName == null || team.members[i].fullName == null || team.members[i].userName == '' || team.members[i].fullName == '' || team.members[i].userName == ' ' || team.members[i].fullName == ' ' || team.members[i].userName == 'Jira users are not loaded' || team.members[i].fullName == 'Jira users are not loaded') {
                    window.alert('is not correct');
                    //valid = false;
                }
            }
        }
        return valid;
    }

    function serverResponse(response) {
        if (response.data.Answer == 'WrongTeam') {
            window.alert('Team is empty');
        }
        if (response.data.Answer == 'WrongName') {
            $uibModal.open({
                animation: true,
                templateUrl: 'modal.html',
                message: 'Team name is incorrect'
            });
            //window.alert('Team name is incorrect');
        }
        if (response.data.Answer == 'MembersIsEmpty') {
            window.alert('No members in team');
        }
        if (response.data.Answer == 'MembersIsNull') {
            window.alert('Some of member is empty');
        }
        if (response.data.Answer == 'MembersIsNotCorrect') {
            window.alert('Username or fullname of member is not correct');
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
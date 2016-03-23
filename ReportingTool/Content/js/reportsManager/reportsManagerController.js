'use strict';

reportsManagerModule.controller("reportsManagerController",
    ['$scope', '$stateParams', '$state', 'ReportsFactory', 'TempObjectFactory', '$filter',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory, $filter) {

            var tempTemplate = TempObjectFactory.get();
            $scope.content = "team";
            $scope.templateValue = tempTemplate.templateName;

            $scope.teams = [{
                teamName: 'Loading...',
                teamID: ''
            }];

            $scope.templates = [{
                templateName: 'Loading...',
                templateId: -1
            }];

            $scope.selectedTeam = {};
            $scope.selectedTemplate = tempTemplate;

            ReportsFactory.getTeams().then(function (response) {
                $scope.teams = response.data;
            }, function (error) {
                console.error("getTeams error!", error);
            });

            ReportsFactory.getTemplates().then(function (response) {
                $scope.templates = response.data;
            }, function (error) {
                console.error("getTemplates error!", error);
            });

            $scope.jiraUsers = [{
                userName: 'Loading...',
                fullName: 'Loading...'
            }];

            ReportsFactory.getJiraUsers().then(getJiraUsersSuccess, getJiraUsersFail);

            function getJiraUsersSuccess(response) {
                $scope.jiraUsers = response.data;
            }

            function getJiraUsersFail(response) {
                console.error("getUsers error!");
            };

            $scope.members = [];

            $scope.addMember = function (member) {
                for (var i in $scope.members) {
                    if ($scope.members[i].userName === member.userName) {
                        return;
                    }
                }
                $scope.members.push(member);
            }

            $scope.removeMember = function (userName) {
                for (var i in $scope.members) {
                    if ($scope.members[i].userName === userName) {
                        $scope.members.splice(i, 1);
                    }
                }
            }

            $scope.fromDate = new Date();
            $scope.toDate = new Date();

            $scope.clear = function () {
                $scope.from = null;
                $scope.to = null;
            };

            $scope.dateOptions = {
                dateDisabled: disabled,
                maxDate: new Date(2050, 5, 22),
                minDate: new Date(1990, 12, 31),
                startingDay: 1
            };

            // Disable weekend selection
            function disabled(data) {
                var date = data.date,
                  mode = data.mode;
                return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
            }

            $scope.open1 = function () {
                $scope.popup1.opened = true;
            };

            $scope.open2 = function () {
                $scope.popup2.opened = true;
            };

            $scope.setDate = function (year, month, day) {
                $scope.dt = new Date(year, month, day);
            };

            $scope.popup1 = {
                opened: false
            };

            $scope.popup2 = {
                opened: false
            };

            function formatDate(date) {
                return $filter('date')(date, "yyyy-MM-dd");
            }

            function isEmpty(obj) {
                if (obj == null) return true;
                if (obj.length > 0) return false;
                if (obj.length === 0) return true;
                for (var key in obj) {
                    if (obj.hasOwnProperty(key))
                        return false;
                }
                return true;
            }

            function initializeTempReportConditionals(templateName, fields) {
                return {                  
                    templateName: templateName,
                    fields: fields,
                    members: $scope.members,
                    teamId: $scope.selectedTeam.teamID,
                    from: formatDate($scope.fromDate),
                    to: formatDate($scope.toDate)
                }
            }

            function saveReportConditionals(tempReportConditionals) {
                TempObjectFactory.set({});
                TempObjectFactory.set(tempReportConditionals);
                $state.go('mainView.reportsManager.reportsConditions.reportDraft');
            }

            $scope.saveIntoClientStorage = function () {
                //TODO: Add here validation about all input values and selected data

                if (isEmpty(tempTemplate)) {
                    var dataPromise = ReportsFactory.getFields($scope.selectedTemplate.templateId);
                    dataPromise.then(function (result) {
                        var tempReportConditionals = initializeTempReportConditionals(result.data.templateName, result.data.fields);
                        saveReportConditionals(tempReportConditionals);
                    });
                } else {
                    var tempReportConditionals = initializeTempReportConditionals(tempTemplate.templateName, tempTemplate.fields);
                    saveReportConditionals(tempReportConditionals);
                }
            }
        }]);

reportsManagerModule.controller("reportDraftController",
    ['$scope', '$stateParams', '$state', 'ReportsFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory) {

           
            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});

        }]);

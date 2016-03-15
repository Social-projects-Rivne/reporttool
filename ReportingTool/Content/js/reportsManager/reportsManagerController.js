'use strict';

reportsManagerModule.controller("reportsManagerController",
    ['$scope', '$stateParams', '$state','ReportsFactory', 'TempObjectFactory','$filter',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory,  $filter) {

            var tempTemplate = TempObjectFactory.get();
            $scope.content = "team";
            $scope.templateValue = tempTemplate.templateName;

            $scope.teams = [{
                teamName: 'Loading...',
                teamID:''
            }];

            $scope.templates = [{
                templateName: 'Loading...',
                templateId: ''
            }];

            $scope.selectedTeam = {};
            $scope.selectedTemplate = {};

            ReportsFactory.getTeams().then(function(response) {
                $scope.teams = response.data;
            }, function(error) {
                console.error("getTeams error!", error);
            });
 
            ReportsFactory.getTemplates().then(function (response) {
                $scope.templates = response.data;
            }, function (error) {
                console.error("getTemplates error!", error);
            });

            $scope.fromDate = new Date();
            $scope.toDate = new Date();

            $scope.clear = function () {
                $scope.from = null;
                $scope.to = null;
            };

            $scope.dateOptions = {
                dateDisabled: disabled,
                maxDate: new Date(2020, 5, 22),
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
                return $filter('date')(date, "dd/MM/yyyy");
            }

            $scope.saveIntoClientStorage = function () {
                //TODO: Add here validation about all input values and selected data
                //TODO: Add checking if all template data haven't come with tempTemplate
                var dataPromise = ReportsFactory.getFields($scope.selectedTemplate.templateId);
                dataPromise.then(function(result) {
                    var tempReportConditionals = {
                        //templateName: $scope.selectedTemplate.templateName,
                        //fields: fields,
                        //template: getFields($scope.selectedTemplate.templateId),
                        template: result.data,
                        teamId: $scope.selectedTeam.teamID,
                        from: formatDate($scope.fromDate),
                        to: formatDate($scope.toDate)
                    };
                    TempObjectFactory.set({});
                    TempObjectFactory.set(tempReportConditionals);
                    $state.go('mainView.reportsManager.reportsConditions.reportDraft');
                });           
            }
        }]);

reportsManagerModule.controller("reportDraftController",
    ['$scope', '$stateParams', '$state','ReportsFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory) {

            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});
        }]);

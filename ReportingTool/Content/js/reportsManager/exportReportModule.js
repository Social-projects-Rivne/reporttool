'use strict';

var exportReportModule = angular.module("exportReportModule", ['ui.bootstrap', 'ngAnimate']);

exportReportModule.controller("exportReportController", ['$scope', '$http', 'exportFactory', function ($scope, $http, exportFactory) {
    $scope.exportToPdf = exportFactory.exportReport;
}]);

exportReportModule.factory('exportFactory', ['$http', function ($http) {
    var exportFactory = {
        exportReport: exportReport
    };

    function exportReport() {
        return $http.get('ExportToPdf?url=' + encodeURIComponent(window.location.href));
    }

    return exportFactory;
}]);
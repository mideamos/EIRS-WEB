var apiUrl = document.getElementById("api-url").innerHTML;
var accessToken = "";

var serviceBillListResult = [];
var assessmentBillListResult = [];

var businessesListResult = [];
var landListResult = [];
var buildingListResult = [];
var vehicleListResult = [];

var lineChart1 = null;
var defaultLineChart1 = null;

var monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
];

Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
};

var settlementStatusNames = [];
var taxPayerTypes = [];
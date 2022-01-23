var UpdateRole = function () {
    var id = $(this).data("id");
    console.log(id);
    var role = $(this).val();
    $("#memberId").val(id);
    $("#memberRole").val(role);
    console.log("Submitting role change request....");
    $("#memberRoleForm").submit();
    console.log("Submitted!");
    console.log($("#memberId").val());
}

var completeRole = function (res) {
    console.log("Role Update Completed");
}

$(".memberRole").on('change', UpdateRole);
$("#searchForm").keyup(function () {
    console.log("search sent!");
    $("#clearIcon").show();
    $("#searchForm").submit();
});

$("#clearIcon").on('click', function () {
    $("#searchQuery").val("");
    $("#searchMemberContainer").empty();
    $(this).hide();
});

$("#searchMemberContainer").on('click', ".userToSelect", function () {
    console.log("Search user selected");
    var userName = $(this).data('user');
    var profilePhotoPath = $(this).data('photo');
    var fullName = $(this).data('name');
    // allow multiple users to be added
    membersValCurr = $("#mainMembers").val();
    newValMembers = "";
    if (membersValCurr) {
        newValMembers = membersValCurr + "," + userName;
    }
    else {
        newValMembers = userName;
    }
    console.log("New project members value:")
    console.log(newValMembers);
    $("#mainMembers").val(newValMembers);
    // reset search query to empty
    $("#searchQuery").val("");
    console.log(userName);
    $("#searchMemberContainer").empty();
    $("#memberPhotos").append(`<div class="chip">
                        <img src="${profilePhotoPath}" alt="Member photo">
                        ${fullName}
                    </div>`)
    // indicate user has been chosen
    $("#searchQuery").data('selected', true);
});


ShowSearchMembersResult = function (res) {
    console.log(res);
    var result = res.responseText;
    console.log(result);
    $("#searchMemberContainer").empty();
    $("#searchMemberContainer").append(result);
}

// clear friend options if user clicks on another screen segment
/*$(document).click(function () {
    if ($("#searchQuery").data('selected') == false) {
        $("#searchQuery").val("");
        $("#searchMemberContainer").empty();
    }
});*/


$(document).ready(function () {
    $("#mainMembers").val("");
})
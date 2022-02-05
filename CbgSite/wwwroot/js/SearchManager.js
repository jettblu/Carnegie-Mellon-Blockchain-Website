$("#searchForm").keyup(function () {
    console.log("search sent!");
    $("#clearIcon").show();
    $("#searchForm").submit();
});

$("#clearIcon").on('click', function () {
    console.log("Clear icon clicked!");
    $("#searchQuery").val("");
    $("#searchMemberContainer").empty();
    $("#searchTagContainer").empty();
    $(this).hide();
});

//FIX AS REDUNDANT.... can be optimized into one function that cases on search data type

$("#tagContainer").on('click', ".tagToSelect", function () {
    console.log("Search user selected");
    var tagId = $(this).data('id');
    var tagName = $(this).data('name');
    // allow multiple tags to be added
    tagsValCurr = $("#mainMembers").val();
    newValtags = "";
    if (tagsValCurr) {
        newValTags = tagsValCurr + "," + tagId;
    }
    else {
        newValMembers = tagId;
    }
    console.log("New tags value:")
    console.log(newValTags);
    $("#searchTags").val(newValTags);
    $("#maintags").val(newValTags);
    // reset search query to empty
    $("#searchQuery").val("");
    console.log(userName);
    $("#searchMemberContainer").empty();
    $("#tagChips").append(`<div class="chip">
                        ${fullName}
                    </div>`);
    // indicate tag has been chosen
    $("#searchQuery").data('selected', true);
});


$("#searchMemberContainer").on('click', ".userToSelect", function () {
    console.log("Search user selected");
    var userName = $(this).data('user');
    var fullName = $(this).data('name');
    var profilePhotoPath = $(this).data('photo');
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
    $("#searchMembers").val(newValMembers);
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


ShowSearchTagsResult = function (res) {
    console.log(res);
    var result = res.responseText;
    console.log(result);
    $("#searchTagContainer").empty();
    $("#searchTagContainer").append(result);
}

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


/*$(document).ready(function () {
    $("#searchMembers").val("");
    $("#mainMembers").val("");
})*/
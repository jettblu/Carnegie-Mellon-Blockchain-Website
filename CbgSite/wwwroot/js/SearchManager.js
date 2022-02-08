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

$("#searchTagContainer").on('click', ".tagToSelect", function () {
    console.log("Search user selected");
    var tagId = $(this).data('id');
    var tagName = $(this).data('name');
    console.log("Tag id:");
    console.log(tagId);
    console.log("Tag name:");
    console.log(tagName);
    // UNCOMMENT BELOW FOR CLIENT SIDE STRING TAG MANAGEMENT
/*    tagsValCurr = $("#mainMembers").val();
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
    $("#maintags").val(newValTags);*/
    // reset search query to empty
    $("#searchQuery").val("");
    $("#searchTagContainer").empty();
    if (tagId != "na") {
        $("#tagOnPost").val(tagId);
    }
    else {
        $("#tagOnPost").val(tagName);
    }
    // indicate tag has been chosen
    $("#searchQuery").data('selected', true);
    $("#addTagForm").submit();
    console.log("Tag add request submitted!");
});

$("#tagChips").on('click', ".chip", ".close", function () {
    tagId = $(this).data("id");
    console.log("Tag id (to remove):");
    console.log(tagId);
    console.log("tag close initiated");
    $("#tagRemoveOnPost").val(tagId);
    $("#removeTagForm").submit();
    $("#searchQuery").val("");
    console.log("Remove tag request submitted!");
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

AddTagResult = function (res) {
    // json response is returned directly as opp to w/in jsonresponsetext
    var result = res.responseJSON;
    console.log(result);
    if (result.isSuccess) {
        var tagName = result.tagContent;
        $("#tagChips").append(`<div class="chip">
                        ${tagName}
                    </div>`);
        console.log("Add tag success!")
    }
    else {
        console.log("Add tag failure");
    }
}


RemoveTagResult = function (res) {
    // json response is returned directly as opp to w/in jsonresponsetext
    var result = res.responseJSON;
    console.log(result);
    if (result.isSuccess) {
        var tagName = result.tagContent;
    }
    else {
        console.log("Remove tag failure");
    }
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
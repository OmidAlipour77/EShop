
//Script_GroupProduct
function Create(parentid) {
    $.get("/Admin/ProductGroups/Create/" + parentid,
        function (result) {
            $("#myModal").modal();
            $("#myModalLabel").html("افزودن گروه جدید");
            $("#modalBody").html(result);
        });
}
function Edit(id) {
    $.get("/Admin/ProductGroups/Edit/" + id,
        function (result) {
            $("#myModal").modal();
            $("#myModalLabel").html("ویرایش گروه");
            $("#modalBody").html(result);
        });
}
function Delete(id) {
    if (confirm("آیا از حذف گروه مطمئن هستید؟")) {
        $.get("/Admin/ProductGroups/Delete/" + id, function () {
            $("#group_" + id).hide("slow");
        });
    }
}
//End Script_GroupProduct


//SCript Feature
function AddFeature() {
    $.get("/Admin/Features/Create/", function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("افزودن ویژگی");
        $("#modalBody").html(result);
    });
}
function EditFeature(id) {
    $.get("/Admin/Features/Edit/" + id, function (result) {
        $("#myModal").modal();
        $("#myModalLabel").html("ویرایش ویژگی");
        $("#modalBody").html(result);
    });
}
function DeleteFeature(id) {
    if (confirm("آیا از حذف این ویژگی مطمئن هستید؟")) {
        $.get("/Admin/Features/Delete/" + id, function () {
            $("#feature_" + id).hide("slow");
        })
    };
}
//End

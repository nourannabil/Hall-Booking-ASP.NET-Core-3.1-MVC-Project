function confirmDelete(Id, isTrue) {
    var deleteButton = 'deleteButton_' + Id;
    var confirmDelete = 'confirmDelete_' + Id;


    if (isTrue) {
        $("#" + deleteButton).hide();
        $("#" + confirmDelete).show();
    } else {
        $("#" + deleteButton).show();
        $("#" + confirmDelete).hide();
    }
}


var productCategoryController = function () {
    this.initialize = function () {
        loadData();
    }

    function loadData() {
        $.ajax({
            url: '/Admin/ProductCategory/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                   
                });
                var treeArr = tedu.unflattern(data);

                //var $tree = $('#treeProductCategory');

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd:true
                });

            }
        });
    }
}
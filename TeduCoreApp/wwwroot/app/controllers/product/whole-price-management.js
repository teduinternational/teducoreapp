var WholePriceManagement = function (parent) {
    var self = this;
    var cachedObj = {
        colors: [],
        sizes: []
    }

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-whole-price', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            loadWholePrices();
            $('#modal-whole-price').modal('show');
        });
        $('body').on('click', '.btn-delete-whole-price', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

        $('#btn-add-whole-price').on('click', function () {
            var template = $('#template-table-whole-price').html();
            var render = Mustache.render(template, {
                Id: 0,
                FromQuantity: 0,
                ToQuantity: 0,
                Price: 0
            });
            $('#table-content-whole-price').append(render);
        });
        $("#btnSaveWholePrice").on('click', function () {
            var priceList = [];
            $.each($('#table-content-whole-price').find('tr'), function (i, item) {
                priceList.push({
                    Id: $(item).data('id'),
                    ProductId: $('#hidId').val(),
                    FromQuantity: $(item).find('input.txtQuantityFrom').first().val(),
                    ToQuantity: $(item).find('input.txtQuantityTo').first().val(),
                    Price: $(item).find('input.txtWholePrice').first().val(),
                });
            });
            $.ajax({
                url: '/admin/Product/SaveWholePrice',
                data: {
                    productId: $('#hidId').val(),
                    wholePrices: priceList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-whole-price').modal('hide');
                    $('#table-content-whole-price').html('');
                }
            });
        });
    }
    function loadWholePrices() {
        $.ajax({
            url: '/admin/Product/GetWholePrices',
            data: {
                productId: $('#hidId').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                var template = $('#template-table-whole-price').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        FromQuantity: item.FromQuantity,
                        ToQuantity: item.ToQuantity,
                        Price: item.Price
                    });
                });
                $('#table-content-whole-price').html(render);
            }
        });
    }
    
}
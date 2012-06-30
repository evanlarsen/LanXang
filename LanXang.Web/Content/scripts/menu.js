function MenuVM(model) {
    var self = this;

    self.Categories = ko.observableArray([]);

    self.addCategory = function () {
        self.Categories.push(new Category());
    }

    self.moveUp = function (category) {
        var position = ko.utils.arrayIndexOf(self.Categories(), category);
        self.Categories.remove(category);
        self.Categories.splice(position - 1, 0, category);
    }

    self.moveDown = function (category) {
        var position = ko.utils.arrayIndexOf(self.Categories(), category);
        self.Categories.remove(category);
        self.Categories.splice(position + 1, 0, category);
    }

    self.canMoveUp = function (category) {
        var position = ko.utils.arrayIndexOf(self.Categories(), category);
        return !!(position > 0);
    }

    self.canMoveDown = function (category) {
        var position = ko.utils.arrayIndexOf(self.Categories(), category);
        return !(position === (self.Categories().length - 1));
    }

    self.deleteCategory = function (category) {
        self.Categories.remove(category);
    }

    self.Categories.subscribe(function () {
        var _cat = self.Categories();
        if (_cat) {
            for (var i = 0, max = _cat.length; i < max; i++) {
                _cat[i].Sequence(i);
            }
        }
    }, self);

    function _init() {
        if (model && model.Categories && model.Categories.length > 0) {
            var mapping = {
                'Categories': {
                    create: function (options) {
                        return new Category(options.data);
                    }
                }
            },
            orderedCategories = model.Categories.sort(function (left, right) {
                return left.Sequence > right.Sequence ? 1 : -1;
            });
            model.Categories = orderedCategories;
            ko.mapping.fromJS(model, mapping, self);
        }
    }

    _init();
}

function Category(data) {
    var self = this;

    self.Sequence = ko.observable();
    self.Name = ko.observable();
    self.Description = ko.observable();
    self.MenuItems = ko.observableArray([]);

    self.isZeroMenuItems = ko.computed(function () {
        if (self.MenuItems()) {
            return !!(self.MenuItems().length === 0)
        }
        return true;
    }, self);

    self.addMenuItem = function () {
        self.MenuItems.push(new MenuItem());
    }

    self.moveUp = function (menuItem) {
        var position = ko.utils.arrayIndexOf(self.MenuItems(), menuItem);
        self.MenuItems.remove(menuItem);
        self.MenuItems.splice(position - 1, 0, menuItem);
    }

    self.moveDown = function (menuItem) {
        var position = ko.utils.arrayIndexOf(self.MenuItems(), menuItem);
        self.MenuItems.remove(menuItem);
        self.MenuItems.splice(position + 1, 0, menuItem);
    }

    self.canMoveUp = function (menuItem) {
        var position = ko.utils.arrayIndexOf(self.MenuItems(), menuItem);
        return !!(position > 0);
    }

    self.canMoveDown = function (menuItem) {
        var position = ko.utils.arrayIndexOf(self.MenuItems(), menuItem);
        return !(position === (self.MenuItems().length - 1));
    }

    self.deleteMenuItem = function (menuItem) {
        self.MenuItems.remove(menuItem);
    }

    self.mvcNameValue = ko.computed(function () {
        return "Categories[" + self.Sequence() + "].";
    }, self);

    self.MenuItems.subscribe(function () {
        var _mi = self.MenuItems();
        if (_mi) {
            for (var i = 0, max = _mi.length; i < max; i++) {
                _mi[i].Sequence(i);
            }
        }
    }, self);

    function _init() {
        if (data) {
            var mapping = {
                'MenuItems': {
                    create: function (options) {
                        return new MenuItem(options.data);
                    }
                }
            };
            if (data.MenuItems && data.MenuItems.length > 0) {
                var orderedMenuItems = data.MenuItems.sort(function (left, right) {
                    return left.Sequence > right.Sequence ? 1 : -1;
                });
                data.MenuItems = orderedMenuItems;
            }
            ko.mapping.fromJS(data, mapping, self);
        }
    }

    _init();
}

function MenuItem(data) {
    var self = this;

    self.Name = ko.observable();
    self.Description = ko.observable();
    self.Price = ko.observable();
    self.Sequence = ko.observable();

    self.mvcNameValue = ko.computed(function () {
        return "MenuItems[" + self.Sequence() + "].";
    }, self);

    function _init() {
        if (data) {
            ko.mapping.fromJS(data, {}, self);
        }
    }

    _init();
}
    

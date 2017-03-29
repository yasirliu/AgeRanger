define(function () {
  'use strict';
  var person = function (id, firstName, lastName, age) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.age = age;
    this.validateResult = [];
  }

  person.prototype.setVersion = function (version) {
    this._rowVersion = version;
  };
  person.prototype.validate = function () {
    if (this.id)
      this.validateResult.push({
        prop: "id",
        msg: "is null"
      });
    else if (parseInt(id) == NaN)
      this.validateResult.push({
        prop: "id",
        msg: "is not integer"
      });
    if (this.firstName)
      this.validateResult.push({
        prop: "firstName",
        msg: "is null"
      });
    if (this.lastName)
      this.validateResult.push({
        prop: "lastName",
        msg: "is null"
      });
    if (this.age)
      this.validateResult.push({
        prop: "age",
        msg: "is null"
      });
    else if (parseInt(this.age) == NaN)
      this.validateResult.push({
        prop: "age",
        msg: "is not integer"
      });
    else if (this.age < 0 && this.age > 2147483647)
      this.validateResult.push({
        prop: "age",
        msg: "is not in [0, 2147483647]"
      });
  };

  return person;
});

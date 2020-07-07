'use strict';

module.exports = {
  generateRandomSale
};

// Make sure to "npm install faker" first.
const Faker = require('faker');

function generateRandomSale(userContext, events, done) {
  
  const name = `${Faker.name.firstName()} ${Faker.name.lastName()}`;
  const productName = Faker.commerce.productName();
  const price = Faker.commerce.price();
  
  userContext.vars.user = name;
  userContext.vars.productName = productName;
  userContext.vars.price = Number(price);
  
  return done();
}
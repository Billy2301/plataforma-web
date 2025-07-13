// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const CUSTOMER = {
    pagoid: "",
    area: "",
    amount: "",
    currency: "",
    description: "",
    hc: "",
    tipodocpago: "",
    razonsocial: "",
    ruc: "",
    direccionpago: "",
    email: "",
    totalpagar: "",
    conDni: "",
    sedeId: "",
    triajeOnId: "",
    detalle: ""
}

const MODELO_ORDEN = {
    order: "",
    description: "",
    amount: "",
    currency: "",
    customer: CUSTOMER,
}

const MODELO_CHARGE = {
    tokenId: "",
    email: "",
    card: "",
    ip: "",
    browser: "",
    devicetype: "",
    installments: "",
    cardbrand: "",
    cardtype: "",
    customer: CUSTOMER,
}

const MODELO_ORDER = {
    orderId: "",
    orderNumber: "",
    paymenCode: "",
    description: "",
    object: "",
    amount: "",
    customer: CUSTOMER
}

const MODELO_VOUCHER = {
    banco: "",
    fechaPago: "",
    nroOperacion: "",
    agencia: "",
    moneda: "",
    nombreImagen: "",
    customer: CUSTOMER
}
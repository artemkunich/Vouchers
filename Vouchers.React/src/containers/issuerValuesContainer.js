import React, {useState, useEffect, useRef, useCallback } from 'react'
import { Header } from '../components/header/header.tsx'
import { IssuerValuesList } from '../issuerValues/issuerValuesList.js'
import { IssuerVouchersList } from '../issuerVouchers/issuerVouchersList.js'
import { ValueDetail } from '../valueDetail/valueDetail.js'

export const IssuerValuesContainer = () => {

    const [selectedValue4Vouchers, setSelectedValue4Vouchers] = useState();
    const [selectedValue4Detail, setSelectedValue4Detail] = useState();

    const handleCancel = useCallback(() => {
        setSelectedValue4Vouchers()
        setSelectedValue4Detail()
    }, [])

    return <>
        {
            selectedValue4Vouchers ? <Header>{selectedValue4Vouchers.ticker} vouchers</Header> :
            selectedValue4Detail ? <Header>{selectedValue4Detail.ticker} detail</Header> :
            <Header>Issuer's values</Header>
        }  
        <div style={{display: selectedValue4Vouchers || selectedValue4Detail ? "none" : "block"}}>
            <IssuerValuesList openValueDetail={setSelectedValue4Detail} openVouchers={setSelectedValue4Vouchers}/>
        </div>

        {selectedValue4Vouchers && <IssuerVouchersList valuesItem={selectedValue4Vouchers} handleCancel={handleCancel}/>}
        {selectedValue4Detail && <ValueDetail valuesItem={selectedValue4Detail} onCancel={handleCancel}/>}
    </>  
}
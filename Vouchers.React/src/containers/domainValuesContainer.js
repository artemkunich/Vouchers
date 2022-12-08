import React, {useState, useEffect, useRef, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { ValueDetail } from '../valueDetail/valueDetail.js'
import { TransactionRequestDetail } from '../holderTransactionRequests/transactionRequestDetail.js'
import { DomainValuesList } from '../domainValues/domainValuesList.js'
import { Header } from '../components/header/header.tsx'

export const DomainValuesContainer = () => {

    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [selectedValue4Detail, setSelectedValue4Detail] = useState()
    const [selectedValue4Request, setSelectedValue4Request] = useState()

    const handleCancel = useCallback(() => {
        setSelectedValue4Detail()
        setSelectedValue4Request()
    }, [])

    return <>
        <Header>Domain values</Header>
        <div style={{display: (selectedValue4Detail || selectedValue4Request) ? "none" : "block"}}>
            <DomainValuesList selectValue4Detail={setSelectedValue4Detail} selectValue4Request={setSelectedValue4Request} />
        </div>
        {
        selectedValue4Detail ? 
            <ValueDetail valuesItem={selectedValue4Detail} onCancel={handleCancel}></ValueDetail>
        : selectedValue4Request ?
            <TransactionRequestDetail transactionRequest={{
                unitTypeId: selectedValue4Request.id,
                unitTicker: selectedValue4Request.ticker,
                unitImageId: selectedValue4Request.imageId,
                unitIssuerAccountId: selectedValue4Request.issuerAccountId,
                unitIssuerEmail: selectedValue4Request.issuerEmail,
                unitIssuerName: selectedValue4Request.issuerName,
                mustBeExchangeable: false,
                dueDate: formatDate(new Date()),
                amount: 0,
                maxDaysBeforeValidityStart: 0,
                minDaysBeforeValidityEnd: selectedValue4Request.issuerAccountId == currentAccount.id ? 0 : 7,
                debtorAccountId: currentAccount.id
            }} handleCancel={handleCancel}></TransactionRequestDetail>
        : <></>
            
        }
    </>
}

const formatDate = (date) => {

    var month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();

    if (month.length < 2) 
        month = '0' + month;
    if (day.length < 2) 
        day = '0' + day;

    return [year, month, day ].join('-');
}
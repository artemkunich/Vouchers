import React, {useState, useEffect, useRef, useMemo, useCallback } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { Header } from '../components/common/header.tsx'
import { HolderTransactionDetailContainer } from '../components/holderTransactionDetail/container.js'
import { DomainAccountsList } from '../components/domainAccounts/list.tsx'
import { HolderValuesList } from '../components/holderValues/list.js'
import "bootstrap-icons/font/bootstrap-icons.css";

export const PagesHolderValues = () => {

    const currentAccount = useSelector(state => state.domainAccount.currentAccount)

    const [selectedValue, setSelectedValue] = useState();
    const [selectedDebtorAccount, setSelectedDebtorAccount] = useState();

    const handleCancel = useCallback(() => {
        setSelectedValue()
        setSelectedDebtorAccount()
    }, [])
    
    return (
        <>
            <div style={{display: selectedValue ? "none" : "block"}}>
                <Header>Holder's values</Header>
                <HolderValuesList handleNewTransaction={setSelectedValue} handleCancel={handleCancel}/>
            </div>
            {
                selectedValue ?
                    selectedDebtorAccount ?
                    <>
                        <Header>New transaction</Header>
                        <HolderTransactionDetailContainer transaction={{
                            creditorAccountId: currentAccount.id,
                            debtorAccountId: selectedDebtorAccount?.id,
                            debtorName: selectedDebtorAccount.name,
                            debtorEmail: selectedDebtorAccount.email,
                            amount: 0,
                            unitTypeId: selectedValue.id,
                            unitIssuerAccountId: selectedValue.issuerAccountId,
                            unitTicker: selectedValue.ticker,
                            items: [],
                            message: ""
                        }} handleCancel={handleCancel}/>
                    </>
                    :
                    <>
                        <Header>Please select recipient</Header>
                        <DomainAccountsList selectDomainAccount={setSelectedDebtorAccount} handleCancel={handleCancel}/>
                    </>
                :
                <></>             
            }           
        </>  
    );
}
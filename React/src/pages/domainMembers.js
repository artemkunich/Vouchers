import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainMembersList } from '../components/domainMembers/list.tsx'
import { Header } from '../components/common/header.tsx'
import { IdentityDetailContainer } from '../components/identityDetail/container.js'

const domainAccountActions = {
    openDetail: "openDetail"
}

export const PagesDomainMembers = () => {

    const [selectedDomainAccount, setSelectedDomainAccount] = useState();

    const openDomainAccountDetail = useCallback((domainAccount) => {
        setSelectedDomainAccount({
            domainAccount,
            action: domainAccountActions.openDetail
        })
    }, [])

    const handleCancel = useCallback(() => {
        setSelectedDomainAccount()
    }, [])

    return <>
        <Header>{selectedDomainAccount ? selectedDomainAccount.domainAccount.name + ' profile' : "Domain's members"}</Header>  
        {
            <>
                <div style={{display: selectedDomainAccount ? "none" : "block"}}>
                    <DomainMembersList openDetail={openDomainAccountDetail}/>
                </div>
                {
                    selectedDomainAccount && <IdentityDetailContainer accountId={selectedDomainAccount.domainAccount.id} handleCancel={handleCancel}/>                
                }
            </>
        }
    </>  
}
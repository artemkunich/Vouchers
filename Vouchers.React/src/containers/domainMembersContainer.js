import React, {useState, useEffect, useRef, useCallback } from 'react'
import { DomainMembersList } from '../domainMembers/domainMembersList.js'
import { Header } from '../components/header/header.tsx'
import { IdentityDetail } from '../identityDetail/identityDetail.js'

const domainAccountActions = {
    openDetail: "openDetail"
}

export const DomainMembersContainer = () => {

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
                    selectedDomainAccount && <IdentityDetail accountId={selectedDomainAccount.domainAccount.id} handleCancel={handleCancel}/>                
                }
            </>
        }
    </>  
}
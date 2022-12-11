import * as React from 'react'
import { ListElement } from '../common/list/list'
import { DomainAccountDto } from '../../types/dtos'
import { getImageSrc } from '../../api/imageSources'
import "bootstrap-icons/font/bootstrap-icons.css";

interface DomainAccountsListProps {
    domainAccount: DomainAccountDto,
    selectDomainAccount: (account: DomainAccountDto) => void,
}

export const DomainAccountsListElement = ({domainAccount, selectDomainAccount} : DomainAccountsListProps) => {

    return <ListElement>
        <div className="container">
            <a onClick={() => selectDomainAccount(domainAccount)}>
                <div className="row mb-2">
                    <div className="col-2 align-middle">
                        { domainAccount.imageId && <img style={{maxHeight: 100, maxWidth: 100}} src={getImageSrc(domainAccount.imageId)}></img> }               
                    </div>            
                    <div className="col-4 align-middle">
                        <p className="fs-5">{domainAccount.email}</p>
                        <p className="fs-5">{domainAccount.name}</p>
                    </div>
                    <div className="col-4 align-middle"/> 
                    <div className="col-2 align-middle"/>    
                </div>
            </a> 
        </div>
    </ListElement>
}
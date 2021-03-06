// Copyright (c) Microsoft. All rights reserved.

import React from "react";
import PropTypes from "prop-types";

import { joinClasses } from "utilities";

export const FormLabel = (props) => {
    const {
            formGroupId,
            className,
            children,
            htmlFor,
            isRequired,
            ...rest
        } = props,
        labelProps = {
            ...rest,
            className: joinClasses("form-group-label", className),
            htmlFor: htmlFor || formGroupId,
        };
    return (
        <label {...labelProps}>
            {children}
            {isRequired ? " *" : ""}
        </label>
    );
};

FormLabel.propTypes = {
    children: PropTypes.node,
    className: PropTypes.string,
    formGroupId: PropTypes.string,
    htmlFor: PropTypes.string,
    type: PropTypes.string,
};

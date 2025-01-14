const config = {
    solution: {
        name: 'Staff Portal',
        tagline: 'Staff Portal',
        dataSource: 'NAV',
        userAvatarGeneral: '/images/user-avatar-general.png',
    },
    client: {
        name:'Kapsowar Mission Hospital',
        logoPath: '/images/client-logo.jpg',
        logoPath2: '/images/client-logo2.jpg',
        bgImagePath: '/images/background.jpg',
    },
    developer: {
      name: 'Potestas Technologies Ltd.',
        logoPath: '/images/client.png',
        websiteUrl: 'https://www.test.com',
        tagline: '',
    },
    errors: {
        generalFailure:"Something went wrong. Kindly refresh the page and try again.",
        dataFetchFailure: "Fetching data failed. If the error persists, contact the system administrator for help.",
        dataDeletionFailure: "Deleting data failed. If the error persists, contact the system administrator for help.",
        validationErrors: "There are validation error(s). Kindly check all the fields."
    },
    baseApiRoute: "/api/",
};

export { config }

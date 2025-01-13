const config = {
    solution: {
        name: 'STAFF PORTAL',
        tagline: 'STAFF PORTAL',
        dataSource: 'NAV',
        userAvatarGeneral: '/images/user-avatar-general.png',
    },
    client: {
        name:'Kenya Redcross Society',
        logoPath: '/images/client-logo.jpg',
        logoPath2: '/images/client-logo-2.png',
        bgImagePath: '/images/background3.jpeg',
    },
    developer: {
      name: 'Kenya Redcross Society',
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
    baseAppRoute: "/hmis/",
};

export { config }
